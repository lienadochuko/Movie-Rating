using System;
using CsvHelper;
using System.Globalization;
using CsvHelper.Configuration;
using OfficeOpenXml;
using System.Drawing;
using OfficeOpenXml.Style;
using System.IO;
using OfficeOpenXml.Drawing;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using System.Diagnostics.Metrics;
using ContactsManager.Core.Domain.RepositoryContracts;
using System.Threading;
using Movie_Rating.ServiceContracts;
using Movie_Rating.Models.DTO;

namespace Movie_Rating.Services
{
    public class MoviesGetterService(IMoviesRepository moviesRepository) : IMoviesGetterServices
    {

        public async Task<IEnumerable<ActorResponse>> GetAllActors(CancellationToken cancellationToken)
        {
            var Actors = await moviesRepository.GetAllActors(cancellationToken);

            IEnumerable<ActorResponse> ActorsResponse = Actors.Select(select => select.ToActorResponse());

            return ActorsResponse;
        }

        public async Task<ActorResponse> GetActorsByID(string actorsID, CancellationToken cancellationToken)
        {
            if (actorsID == null)
                return null;

            ActorsDto Actors = await moviesRepository.GetActors(actorsID, cancellationToken);
            if (Actors == null)
                return null;


            return Actors.ToActorResponse();
        }

        public async Task<MemoryStream> GetActorCSV(CancellationToken cancellationToken)
        {
            MemoryStream memoryStream = new();
            StreamWriter streamWriter = new(memoryStream);


            CsvConfiguration csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture);

            CsvWriter csvWriter = new CsvWriter(streamWriter, csvConfiguration, leaveOpen: true);

            //csvWriter.WriteHeader<ActorResponse>(); //using the model fields as heads such as ActorID, ActorName ...... 

            //await csvWriter.WriteRecordsAsync(Actors);
            ////1, dan, dan@gmail.com ........

            //csvWriter.WriteField(nameof(ActorResponse.ActorID));
            csvWriter.WriteField(nameof(ActorResponse.FirstName));
            csvWriter.WriteField(nameof(ActorResponse.FamilyName));
            csvWriter.WriteField(nameof(ActorResponse.FullName));
            csvWriter.WriteField(nameof(ActorResponse.DOB));
            csvWriter.WriteField(nameof(ActorResponse.Age));
            csvWriter.WriteField(nameof(ActorResponse.DOD));
            csvWriter.WriteField(nameof(ActorResponse.Gender));
            csvWriter.NextRecord(); //goes to the next line (\n)

            IEnumerable<ActorResponse> Actors = await GetAllActors(cancellationToken);

            foreach (ActorResponse Actor in Actors)
            {
                //csvWriter.WriteField(Actor.ActorID);
                csvWriter.WriteField(Actor.FirstName);
                csvWriter.WriteField(Actor.FamilyName);
                csvWriter.WriteField(Actor.FullName);
                csvWriter.WriteField(Actor.DOB.HasValue ? Actor.DOB.Value.ToString("yyyy-MM-dd") : "");
                csvWriter.WriteField(Actor.Age);
                csvWriter.WriteField(Actor.DOD.HasValue ? Actor.DOB.Value.ToString("yyyy-MM-dd") : "");
                csvWriter.WriteField(Actor.Gender);
                csvWriter.NextRecord(); //goes to the next line (\n)
            }


            await csvWriter.FlushAsync(); //when the buffer in the stremwriter gets filled up it flushes to stream, At the end of writing,
                                          //you need to flush the writer so anything in the buffer gets written to the stream ensuring there is no missing record

            memoryStream.Position = 0;

            return memoryStream;
        }

        public async Task<MemoryStream> GetActorExcel(CancellationToken cancellationToken)
        {
            MemoryStream memoryStream = new();

            using (ExcelPackage excelpackage = new(memoryStream))
            {
                var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "image", "splash1.png");
                ExcelWorksheet workSheet = excelpackage.Workbook.Worksheets.Add("ActorsSheet");
                if (File.Exists(imagePath))
                {
                    // Add the image to the Excel file
                    FileInfo image = new(imagePath);
                    ExcelPicture picture = workSheet.Drawings.AddPicture("ImageName", image);
                    picture.SetSize(60);
                    picture.SetPosition(1, 0, 6, 0); // Adjust the position as needed
                }
                else
                {
                    // Handle the case when the image file does not exist
                    // You can log a message, throw an exception, etc.
                }

                using (ExcelRange excelRange = workSheet.Cells["A1:H1"])
                {
                    excelRange.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;

                }

                //Set Headings
                workSheet.Cells["A1"].Value = "Actor Name";
                workSheet.Cells["B1"].Value = "Email";
                workSheet.Cells["C1"].Value = "Gender";
                workSheet.Cells["D1"].Value = "Date of Birth";
                workSheet.Cells["E1"].Value = "Age";
                workSheet.Cells["F1"].Value = "Address";
                workSheet.Cells["G1"].Value = "Country";
                workSheet.Cells["H1"].Value = "Recieve NewsLetter";


                //Align Headings
                workSheet.Cells["A1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["B1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["C1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["D1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["E1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["F1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["G1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                workSheet.Cells["H1"].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                //Bold
                workSheet.Cells["A1"].Style.Font.Bold = true;
                workSheet.Cells["B1"].Style.Font.Bold = true;
                workSheet.Cells["C1"].Style.Font.Bold = true;
                workSheet.Cells["D1"].Style.Font.Bold = true;
                workSheet.Cells["E1"].Style.Font.Bold = true;
                workSheet.Cells["F1"].Style.Font.Bold = true;
                workSheet.Cells["G1"].Style.Font.Bold = true;
                workSheet.Cells["H1"].Style.Font.Bold = true;
                //Set Pattern and BackgroundColor 
                workSheet.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["B1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["C1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["C1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["D1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["D1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["E1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["E1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["F1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["F1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["G1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["G1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);

                workSheet.Cells["H1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                workSheet.Cells["H1"].Style.Fill.BackgroundColor.SetColor(Color.Gray);


                int row = 4;

                IEnumerable<ActorResponse> ActorResponses = await GetAllActors(cancellationToken);

                foreach (ActorResponse Actor in ActorResponses)
                {
                    workSheet.Cells[row, 1].Value = Actor.FirstName;
                    workSheet.Cells[row, 2].Value = Actor.FamilyName;
                    workSheet.Cells[row, 3].Value = Actor.FullName;
                    workSheet.Cells[row, 4].Value = Actor.DOB.HasValue ? Actor.DOB.Value.ToString("yyyy-MM-dd") : "";
                    workSheet.Cells[row, 5].Value = Actor.Age;
                    workSheet.Cells[row, 4].Value = Actor.DOD.HasValue ? Actor.DOB.Value.ToString("yyyy-MM-dd") : "";
                    workSheet.Cells[row, 6].Value = Actor.Gender;

                    workSheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    workSheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    workSheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    workSheet.Cells[row, 4].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                    workSheet.Cells[row, 5].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                    workSheet.Cells[row, 6].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                    row++;
                }

                workSheet.Cells[$"A1:H{row}"].AutoFitColumns();

                await excelpackage.SaveAsync();

                memoryStream.Position = 0;
                return memoryStream;
            }
        }
    }
}
