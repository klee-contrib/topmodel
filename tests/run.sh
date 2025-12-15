#!/bin/bash

# Génération des fichiers TopModel
dotnet run --project ../TopModel.Generator -- --check --file ./model/topmodel.config

# Vérification que la génération s'est bien passée
if [ $? -ne 0 ]; then
    echo "Erreur lors de la génération TopModel"
    exit 1
fi

cd output/jpa || exit 1
mvn clean compile --quiet

cd ../angular
npm run build

cd ../focus
npm run build