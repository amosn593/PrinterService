using Spire.Pdf;
using Spire.Pdf.Fields;

namespace App.WindowsService;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;

    public Worker(ILogger<Worker> logger)
    {
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            PrintDownloadedReceipt();

            await Task.Delay(500, stoppingToken);
        }
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PrinterWorkerService, starting Time: {time}", DateTimeOffset.Now);
        return base.StartAsync(cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("PrinterWorkerService, Stopping Time: {time}", DateTimeOffset.Now);
        return base.StopAsync(cancellationToken);
    }

    public void PrintDownloadedReceipt()
    {
        try
        {
            //Create a PdfDocument object
            PdfDocument doc = new PdfDocument();

            //Load a PDF file 
            //doc.LoadFromBytes(bytes)
            var FilePath = @"C:\POSPrinterService";

            //Check if directory exists, if not create another one
            if (!Directory.Exists(FilePath))
            {
                Directory.CreateDirectory(FilePath);
                _logger.LogInformation("PrinterWorkerService, Created Directory: {filepath} , Time: {time}", FilePath, DateTimeOffset.Now);
            }

            DirectoryInfo DirectoryInfo = new DirectoryInfo(FilePath);


            string ReceiptPatternName = @"*.pdf";

            FileInfo[] Files = DirectoryInfo.GetFiles(ReceiptPatternName);


            foreach (FileInfo file in Files)
            {
                if (File.Exists(file.FullName))
                {
                    if (file.FullName.Contains("RECEIPT"))
                    {
                        doc.LoadFromFile(file.FullName);

                        ////Specify printer name
                        //doc.PrintSettings.PrinterName = "POS80";

                        //Silent printing
                        //doc.PrintSettings.PrintController = new StandardPrintController();

                        //Print document
                        doc.Print();

                        File.Delete(file.FullName);

                        _logger.LogInformation("PrinterWorkerService, Printed Receipt and Deleted the Receipt, Time: {time}", DateTimeOffset.Now);

                        return;
                    }
                    
                }
            }

        }
        catch (Exception ex)
        {
            _logger.LogError("PrinterWorkerService, Error: {error}, Time: {time}", ex.Message, DateTimeOffset.Now);
        }
    }
}
