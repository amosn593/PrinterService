## Automatic POS Printer Service
A printer service to print point of sale(POS) sales receipt(pdf only), 56mm and 88mm thermal printer.

### How to set up
1. Run the installer File(.exe file)
The application(background service) will start automattically
Find the .exe file in the release section
```
C:\POSPrinterService
This Folder is created atomatically by the application
```
**Note**
The FrontEnd(Client Application) must generate the receipt in pdf containing name RECEIPT and save to the above specified folder in the host computer.
Example of Receipt name.
```
RECEIPT.pdf, Tenzi_RECEIPT.pdf, RECEIPT_Test.pdf among other names
```

2. Set the frontend application/browser to download receipts in pdf to this directory.
The Applications prints to your default printer.
If you want to clone the project and test it locally, continue with the following steps
```
C:\POSPrinterService
```

3. Clone the project, build and restore all the packages.
```
.net 8 sdk is required, for the project to run
```
Packages and their versions
```
<PackageReference Include="Microsoft.Extensions.Hosting" Version="8.0.0" />
<PackageReference Include="FreeSpire.PDF" Version="8.6.0" />
<PackageReference Include="Microsoft.Extensions.Hosting.WindowsServices" Version="8.0.0" />
```

4. Publish the app
```
Ensure that the following settings are specified:

Deployment mode: Self-contained
Produce single file: checked
Enable ReadyToRun compilation: checked
```

5. Create the Windows Service,(use your binpath and app name)
```
sc.exe create "POSPrinterService" binpath= "E:\C# - Projects\Published_Projects\PrinterService\PrinterWindowService.exe"
```

6. Start the service
```
sc.exe start "POSPrinterService"
```

7. Create a Windows Service installer
Install tooling dependencies
I am using microsoft installer extension
```
dotnet add  package CliWrap
```

8. Debugging the application
Incase the application is not printing, restart the application from the windows services page, and set it to start automatically.
