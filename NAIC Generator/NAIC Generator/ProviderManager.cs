using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace naic
{
    public class ProviderManager
    {
        /**
        \brief
            Loads providers from the
            given provider file

        \param path
            Path to provider file

        \return
            A list containing all providers
            stored in the given file, or
            null if it could not be loaded
        */
        public List<Provider> LoadFromFile(string path)
        {
            // List of providers that we will load
            // and then output
            List<Provider> output = new List<Provider>();

            // Try to open the given XML file
            // and create an XmlReader object
            XmlReader reader;

            try
            {
                // Create a reader for the given
                // file
                reader = XmlReader.Create(path);

                // Create a serializer to read the
                // objects from XML
                XmlSerializer serializer;
                serializer = new XmlSerializer(typeof(List<Provider>));

                // Read the providers from the XML file
                output = (List<Provider>)serializer.Deserialize(reader);

                // Close the XML reader
                // object
                reader.Close();
            }
            catch (Exception e)
            {
                // TODO Handle this

                return null;
            }

            return output;
        }

        /**
        \brief
            Writes the given providers
            to the file, overwriting
            all previous content.

        \param providers
            List containing providers to
            write

        \param path
            Path to output file. Will overwrite
            existing files.

        \return
            True on success, false on failure
        */
        public bool WriteToFile(
            List<Provider> providers,
            string path)
        {
            // XML writer to write
            // provider file
            XmlWriter writer;

            try
            {
                // Create a reader for the given
                // file
                writer = XmlWriter.Create(path);

                // Create a serializer to write
                // the objects to XML
                XmlSerializer serializer = new XmlSerializer(typeof(List<Provider>));

                // Serialize the object to XML
                serializer.Serialize(writer, providers);

                // Close the writer
                writer.Close();
            }
            catch(Exception e)
            {
                // TODO handle exceptions

                return false;
            }

            return true;
        }
    }
}
