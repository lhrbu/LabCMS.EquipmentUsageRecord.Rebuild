cd .\LabCMS.Gateway.Server\
dotnet publish -c Release -r win10-x64 -p:ReadyToRun=true -p:PublishSingleFile=true -p:DeleteExistingFiles=true --no-self-contained -o ..\Publish\LabCMS.Gateway.Server\
cd ..\LabCMS.EquipmentUsageRecord.Server\
dotnet publish -c Release -r win10-x64 -p:ReadyToRun=true -p:PublishSingleFile=true -p:DeleteExistingFiles=true --no-self-contained -o ..\Publish\LabCMS.EquipmentUsageRecord.Server\
cd ..