////////////////////////////////////////////////////////////////////////////////
//
//    Read Tags with Tag Select Filtering
//
////////////////////////////////////////////////////////////////////////////////

using System;
using Impinj.OctaneSdk;
using OctaneSdk.Impinj.OctaneSdk;

namespace OctaneSdkExamples
{
    class Program
    {
        // Create an instance of the ImpinjReader class.
        static ImpinjReader reader = new ImpinjReader();

        static void Main(string[] args)
        {
            try
            {
                // Connect to the reader.
                // Pass in a reader hostname or IP address as a 
                // command line argument when running the example
                if (args.Length != 1)
                {
                    Console.WriteLine("Error: No hostname specified.  Pass in the reader hostname as a command line argument when running the Sdk Example.");
                    return;
                }
                string hostname = args[0];
                reader.Connect(hostname);

                // Get the default settings
                // We'll use these as a starting point
                // and then modify the settings we're 
                // interested in.
                Settings settings = reader.QueryDefaultSettings();

                // Tell the reader to include the antenna number
                // in all tag reports. Other fields can be added
                // to the reports in the same way by setting the 
                // appropriate Report.IncludeXXXXXXX property.
                settings.Report.IncludeAntennaPortNumber = true;

                // Send a tag report for every tag read.
                settings.Report.Mode = ReportMode.Individual;

                // Set the filter mode.
                // Apply the filters that are specified in the tag select filter list.
                settings.Filters.Mode = TagFilterMode.UseTagSelectFilters;

                // This filter will select the tag on match and deselect if the tag doesn't match.
                // It filters on the first hex character in the EPC memory.
                // The filter is looking to match 3.
                TagSelectFilter tagSelectFilter1 = new TagSelectFilter
                {
                    MatchAction = StateUnawareAction.Select,
                    NonMatchAction = StateUnawareAction.Unselect,
                    TagMask = "3",
                    BitPointer = BitPointers.Epc + 0,
                    MemoryBank = MemoryBank.Epc
                };

                // This filter will select the tag on match and deselect if the tag doesn't match.
                // It filters on the second hex character in the EPC memory.
                // The filter is looking to match 0.
                TagSelectFilter tagSelectFilter2 = new TagSelectFilter
                {
                    MatchAction = StateUnawareAction.Select,
                    NonMatchAction = StateUnawareAction.Unselect,
                    TagMask = "0",
                    BitPointer = BitPointers.Epc + 4,
                    MemoryBank = MemoryBank.Epc
                };

                // This filter will select the tag on match and deselect if the tag doesn't match.
                // It filters on the third hex character in the EPC memory.
                // The filter is looking to match 0.
                TagSelectFilter tagSelectFilter3 = new TagSelectFilter
                {
                    MatchAction = StateUnawareAction.Select,
                    NonMatchAction = StateUnawareAction.Unselect,
                    TagMask = "0",
                    BitPointer = BitPointers.Epc + 8,
                    MemoryBank = MemoryBank.Epc
                };

                // This filter will select the tag on match and deselect if the tag doesn't match.
                // It filters on the fourth hex character in the EPC memory.
                // The filter is looking to match 8.
                TagSelectFilter tagSelectFilter4 = new TagSelectFilter
                {
                    MatchAction = StateUnawareAction.Select,
                    NonMatchAction = StateUnawareAction.Unselect,
                    TagMask = "8",
                    BitPointer = BitPointers.Epc + 12,
                    MemoryBank = MemoryBank.Epc
                };

                // Add newly created filters to extended tag filter list.
                // Altogether, it will only select tags the pass every filter.
                settings.Filters.TagSelectFilters.Add(tagSelectFilter1);
                settings.Filters.TagSelectFilters.Add(tagSelectFilter2);
                settings.Filters.TagSelectFilters.Add(tagSelectFilter3);
                settings.Filters.TagSelectFilters.Add(tagSelectFilter4);

                // Apply the newly modified settings.
                reader.ApplySettings(settings);

                // Assign the TagsReported event handler.
                // This specifies which method to call
                // when tags reports are available.
                reader.TagsReported += OnTagsReported;

                // Start reading.
                reader.Start();

                // Wait for the user to press enter.
                Console.WriteLine("Press enter to exit.");
                Console.ReadLine();

                // Stop reading.
                reader.Stop();

                // Disconnect from the reader.
                reader.Disconnect();
            }
            catch (OctaneSdkException e)
            {
                // Handle Octane SDK errors.
                Console.WriteLine("Octane SDK exception: {0}", e.Message);
            }
            catch (Exception e)
            {
                // Handle other .NET errors.
                Console.WriteLine("Exception : {0}", e.Message);
            }
        }

        static void OnTagsReported(ImpinjReader sender, TagReport report)
        {
            // This event handler is called asynchronously 
            // when tag reports are available.
            // Loop through each tag in the report 
            // and print the data.
            foreach (Tag tag in report)
            {
                Console.WriteLine("EPC : {0} Antenna : {1}",
                                    tag.Epc, tag.AntennaPortNumber);
            }
        }
    }
}
