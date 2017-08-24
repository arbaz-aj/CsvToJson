using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CsvToJsonV3
{   //This class will convert csv to json
    class CsvToJson
    {
        int totalMales,totalFemales, totalIlliteratePerson, totalLiteratePerson, totalIlliterateMalesNE, totalIlliterateFemalesNE, totalLiterateMalesNE, 
            totalLiterateFemalesNE;
        Dictionary<String, int> stateWiseIllitrate = new Dictionary<string, int>();//state as key and illiterate_persons as value
        Dictionary<String, int> stateWiseLiterate = new Dictionary<string, int>();//state as key and Literate_persons as value
        StringBuilder jsonBuilderSB = new StringBuilder();//For Building Json

        List<String> northEasternlist = new List<string> { "State - ARUNACHAL PRADESH", "State - ASSAM", "State - NAGALAND", "State - MEGHALAYA", "State - MANIPUR", "State - TRIPURA", "State - MIZORAM" };

        public void JsonParser()
        {
            StreamReader csvFilereader = new StreamReader(new FileStream(@"..\\Data\\India2011.csv", FileMode.Open));
            csvFilereader.ReadLine(); //Reading The Heading Row
            ValueSetter(csvFilereader);//call ValueSetter function for reading and setting the variables from General census file
            csvFilereader = new StreamReader(new FileStream(@"..\\Data\\IndiaSC2011.csv", FileMode.Open));
            csvFilereader.ReadLine();//reads the headings
            ValueSetter(csvFilereader);//call ValueSetter function for reading and setting the variables from SC census file
            csvFilereader = new StreamReader(new FileStream(@"..\\Data\\IndiaST2011.csv", FileMode.Open));
            csvFilereader.ReadLine();//reads the headings
            ValueSetter(csvFilereader);//call ValueSetter function for reading and setting the variables from ST census file
            csvFilereader.Dispose();//releases all resources held by the streamReader

            jsonBuilderSB.Append("[{\"name\":\"Males\",\"population\":" + totalMales + "},{\"name\":\"Females\",\"population\":" + totalFemales + "}]");
            //will write jsonSB in a file
            JsonFileWriter(@"..\\Data\\IndiaCensusGenderRatio.json", jsonBuilderSB);
            jsonBuilderSB.Clear();

            jsonBuilderSB.Append("[{\"name\":\"illiteratePersons\",\"population\":" + totalIlliteratePerson + "},{\"name\":\"literatePersons\"," +
                "\"population\":" + totalLiteratePerson + "}]");
            JsonFileWriter(@"..\\Data\\IndiaCensusLiteracyRatio.json",jsonBuilderSB);
            jsonBuilderSB.Clear();

            jsonBuilderSB.Append("[{\"name\":\"illiterateMalesNe\",\"population\":" + totalIlliterateMalesNE + "},{\"name\":\"illiteratefemalesNE\"," +
                "\"population\":" + totalIlliterateFemalesNE + "},{\"name\":\"literateMalesNE\",\"population\":" + totalLiterateMalesNE + "},{\"name\":" +
                "\"literatefemalesNE\",\"population\":" + totalLiterateFemalesNE + "}]");
            JsonFileWriter(@"..\\Data\\IndiaCensusLiteracyRatioNE.json", jsonBuilderSB);
            jsonBuilderSB.Clear();

            Int16 count = 0;
            jsonBuilderSB.Append("[");
            foreach (KeyValuePair<string, int> entry in stateWiseIllitrate)
            {
                jsonBuilderSB.Append("{\"name\":\"" + entry.Key + "\",\"illiterate\":" + entry.Value + ",\"literate\":" +stateWiseLiterate[entry.Key]+"}");
                count++;
                if (count != stateWiseIllitrate.Count)//So that ',' is not appended after the last object
                    jsonBuilderSB.Append(",");
            }
            jsonBuilderSB.Append("]");
            JsonFileWriter(@"..\\Data\\IndiaCensusLiteracyRatioStateWise.json", jsonBuilderSB);
            jsonBuilderSB.Clear();
        }//end of JsonParser()

        private void ValueSetter(StreamReader reader)
        {
            while (!reader.EndOfStream)
            {
                String[] data = reader.ReadLine().ToString().Split(separator: ',');//Row of csv file is converted to and array of Strings

                if (data[5] == "All ages" && data[4] == "Total")
                {
                    totalMales += int.Parse(data[7]);
                    totalFemales += int.Parse(data[8]);
                    totalIlliteratePerson += int.Parse(data[9]);
                    totalLiteratePerson += int.Parse(data[12]);

                    String stateKey = data[3].Split('-')[1].Remove(0, 1);
                    if (!stateWiseIllitrate.ContainsKey(stateKey))
                        stateWiseIllitrate.Add(stateKey, int.Parse(data[9]));
                    else  
                        stateWiseIllitrate[stateKey] += int.Parse(data[9]);
                    
                    String stateKeyLit = data[3].Split('-')[1].Remove(0, 1);
                    if (!stateWiseLiterate.ContainsKey(stateKey))
                        stateWiseLiterate.Add(stateKey, int.Parse(data[12]));   
                    else                    
                        stateWiseLiterate[stateKey] += int.Parse(data[12]);
                    
                    if (northEasternlist.Find(m => m == data[3]) == null ? false : true)
                    {
                        totalIlliterateMalesNE += int.Parse(data[10]);
                        totalIlliterateFemalesNE += int.Parse(data[11]);
                        totalLiterateMalesNE += int.Parse(data[13]);
                        totalLiterateFemalesNE += int.Parse(data[14]);
                    }
                }//end of if
            }//end of while
            reader.DiscardBufferedData();
        }//end of valuesetter()

        private void JsonFileWriter(String path, StringBuilder jsonBuilder)
        {
            StreamWriter jsonWriter = new StreamWriter(new FileStream(path, FileMode.OpenOrCreate));
            jsonWriter.WriteLine(jsonBuilder.ToString());
            jsonWriter.Flush();
            jsonWriter.Dispose();
            jsonBuilder.Clear();
        }
    }//end of CsvToJson class
}//end of CsvToJsonV3

