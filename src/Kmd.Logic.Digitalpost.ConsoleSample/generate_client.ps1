iwr http://localhost:54095/swagger/digitalpost/swagger.json -o digitalpost.json
autorest --input-file=digitalpost.json --csharp --output-folder=Client --override-client-name=DigitalPostClient --namespace=Kmd.Logic.Digitalpost.ConsoleSample.Client --add-credentials
