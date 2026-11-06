#!/bin/bash
wslc run -d --rm --name topmodel-tests-db \
    -p 5962:5432 \
    -e POSTGRES_PASSWORD=demo -e POSTGRES_USER=demo -e POSTGRES_DB=demo \
    -v "$(pwd -W)/output/pg/src:/docker-entrypoint-initdb.d" \
    postgres:latest
if [ $? -ne 0 ]; then
    echo "Erreur lors du démarrage du conteneur PostgreSQL"
    exit 1
fi

dotnet run --project ../TopModel.ModelGenerator -- --check --file ./tmdgen/database/tmdgen.config --file ./tmdgen/open-api/tmdgen.config -s
# # Vérification que la génération s'est bien passée
if [ $? -ne 0 ]; then
    echo "Erreur lors de la génération TopModel - tmdgen"
    exit 1
fi
wslc rm -f topmodel-tests-db

# Génération des fichiers TopModel
dotnet run --project ../TopModel.Generator -- --check --schema --file ./model/topmodel.config --file ./tmdgen/database/topmodel.config --file ./tmdgen/open-api/topmodel.config -s

# Vérification que la génération s'est bien passée
if [ $? -ne 0 ]; then
    echo "Erreur lors de la génération TopModel - modgen"
    exit 1
fi

cd output/jpa || exit 1
mvn clean compile --quiet

cd ../angular
npm run build

cd ../focus
npm run build

cd ../csharp/src
dotnet build
