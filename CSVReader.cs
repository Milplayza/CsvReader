using System.IO;

namespace Csv_Reader
{
    public static class CSVReader
    {

        private static int currentColumn;
        private static int substringStart;
        private static int substringEnd;
        private static bool readKommaAsText;
        private static bool entryWithSpecialCharacters;

        /// <summary>
        /// Read an entry from a file
        /// </summary>
        /// <param name="path">path to the file</param>
        /// <param name="line">wanted line (0 indexed)</param>
        /// <param name="column">wanted column (0 indexed)</param>
        /// <returns></returns>
        public static string ReadEntry(string path, int line, int column)
        {
            string entry = string.Empty;
            bool endOfFile = false;
            int lineCount = 0;
            StreamReader sr = new StreamReader(path);

            while (!endOfFile)
            {
                //Handle Line
                string data_string = sr.ReadLine();

                if (data_string == null) return entry;

                if (lineCount == line)
                {
                    entry = GetEntry(data_string, column);
                    return entry;
                }

                lineCount++;
            }

            return entry;
        }

        private static string GetEntry(string lineString, int wantedColumn)
        {
            string entry;

            currentColumn = 0;
            substringStart = 0;
            substringEnd = lineString.Length - 1;
            readKommaAsText = false;
            entryWithSpecialCharacters = false;

            //Foreach char in Line
            for (int i = 0; i < lineString.Length; i++)
            {
                if (lineString[i] == '"') HandleQuotationMarks(wantedColumn, currentColumn);
                if (lineString[i] == ',') HandleComma(wantedColumn, i);
            }

            //Entries with special Characters are in quotation marks e.g.: "Blblblbla"
            if (entryWithSpecialCharacters)
            {
                substringStart++;
                substringEnd--;
            }

            int entryLength = substringEnd + 1 - substringStart;
            entry = lineString.Substring(substringStart, entryLength); //Index stands for language
            return entry;
        }

        private static void HandleQuotationMarks(int wantedColumn, int currentColumn)
        {
            readKommaAsText = !readKommaAsText;
            if (currentColumn == wantedColumn) entryWithSpecialCharacters = true;
        }

        private static void HandleComma(int wantedColumn, int charIndex)
        {
            if (readKommaAsText) return;

            if (currentColumn == wantedColumn) 
                substringEnd = charIndex - 1;

            //Start new Column
            currentColumn++;

            if (currentColumn == wantedColumn)
                substringStart = charIndex + 1;
        }

       
    }
}


// By Milplayza (20. July 2025)