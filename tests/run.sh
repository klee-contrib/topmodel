#!/bin/bash
docker compose up -d db
if [ $? -ne 0 ]; then
    echo "Erreur lors du démarrage du conteneur PostgreSQL"
    exit 1
fi

dotnet run --project ../TopModel.ModelGenerator -- --check --file ./tmdgen/database/tmdgen.config --file ./tmdgen/open-api/tmdgen.config -s
# Vérification que la génération s'est bien passée
if [ $? -ne 0 ]; then
    echo "Erreur lors de la génération TopModel - tmdgen"
    exit 1
fi
docker compose down

# Génération des fichiers TopModel
dotnet run --project ../TopModel.Generator -- --check --file ./model/topmodel.config --file ./tmdgen/database/topmodel.config --file ./tmdgen/open-api/topmodel.config -s

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
