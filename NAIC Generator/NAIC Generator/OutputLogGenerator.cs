﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Include OpenXML library
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Packaging;

namespace naic
{
    /**
    \brief
        Generates an output log that
        describes each NAIC document
        generated by the NAIC generator
    */
    public class OutputLogGenerator
    {
        /// Contains each job that was
        /// handled by the generator
        public List<Job> JobList;

        /// Name of user who generated
        /// NAIC documents
        public string UserName;

        /// Selected provider
        public Provider Provider;

        /// Generated course
        public Course Course;

        /**
        \brief
            Constructor.
        */
        public OutputLogGenerator()
        {
            // Create empty list for jobs
            this.JobList = new List<Job>();
        }

        /**
        \brief
            Generates an output log file
            to the given path.

        \param outputPath
            File path for output log file

        \return
            True on success, false otherwise
        */
        public bool Generate(string outputPath)
        {
            using (WordprocessingDocument document = WordprocessingDocument.Create(
                    outputPath,
                    WordprocessingDocumentType.Document)
                )
            {
                // Add main document part
                // and body
                MainDocumentPart mainPart = document.AddMainDocumentPart();

                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Define some document properties
                FontSize titleFontSize = new FontSize();
                titleFontSize.Val = "36";

                FontSize headerFontSize = new FontSize();
                headerFontSize.Val = "30";

                FontSize mainFontSize = new FontSize();
                mainFontSize.Val = "22";

                FontSize smallFontSize = new FontSize();
                smallFontSize.Val = "19";

                FontSize tinyFontSize = new FontSize();
                smallFontSize.Val = "17";

                RunFonts headerFont = new RunFonts();
                headerFont.Ascii = "Times New Roman";

                RunFonts mainFont = new RunFonts();
                mainFont.Ascii = "Times New Roman";

                RunFonts emphasisFont = new RunFonts();
                mainFont.Ascii = "Arial";

                // Add output header
                Paragraph headerParagraph = body.AppendChild(new Paragraph());
                Run headerRun = headerParagraph.AppendChild(new Run());
                Paragraph headerSubtitleParagraph = body.AppendChild(new Paragraph());
                Run headerSubtitleRun = headerSubtitleParagraph.AppendChild(new Run());

                headerRun.RunProperties = new RunProperties();
                headerRun.RunProperties.FontSize = titleFontSize;
                headerRun.RunProperties.RunFonts = headerFont;
                headerRun.AppendChild(new Text("NAIC Generator"));

                headerSubtitleRun.RunProperties = new RunProperties();
                headerSubtitleRun.RunProperties.FontSize = headerFontSize;
                headerSubtitleRun.RunProperties.RunFonts = emphasisFont;
                headerSubtitleRun.AppendChild(new Text("Output Log"));

                // Add generation information table
                //Paragraph generationTableParagraph = body.AppendChild(new Paragraph());
                Table generationTable = new Table();

                TableRow generationTableHeaderRow = new TableRow();
                TableCell generationTableTitleCell = new TableCell();

                generationTableTitleCell.Append(new Paragraph(new Run(new Text("Generation Settings"))));
                generationTableHeaderRow.Append(generationTableTitleCell);

                generationTable.Append(generationTableHeaderRow);
                body.AppendChild(generationTable);






            }

            return true;
        }
    }
}
