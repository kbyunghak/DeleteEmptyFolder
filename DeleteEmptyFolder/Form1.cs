using System;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;
using System.Configuration;

namespace DeleteEmptyFolder
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            /* Get the default path from App.config and let it be the target folder. */
            textBox2.Text = ConfigurationSettings.AppSettings["defaultPath"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /* Update target folder path from user input */
            string path = textBox2.Text;
            string[] dirs = { };
            
            /* Validate target folder path. Show a message to the user and reset variable if the given path is invalid. */
            try
            {
                /* Get directories contained in the target folder path. */
                dirs = Directory.GetDirectories(path);
            }
            catch (Exception err)
            {
                Console.WriteLine(err.Message);
                textBox1.AppendText("This is not a valid folder path. Please enter a valid path." + Environment.NewLine);
                textBox2.Text = "";
            }

            /* Check each directory contained in the path. */
            foreach (string dir in dirs)
            {
                /* For each directory, get its subdirectories. */
                string[] subDirs = Directory.GetDirectories(dir);

                /* Check each subdirectory. */
                foreach (string subDir in subDirs)
                {
                    /* For each subdirectory, get its files information. */
                    DirectoryInfo subDirInfo = new DirectoryInfo(subDir);
                    FileInfo[] files = subDirInfo.GetFiles();

                    /* If there are no files in this subdirectory, then delete the subdirectory. */
                    if (files.Length < 1)
                    {
                        textBox1.AppendText("Deleting " + subDir + "." + Environment.NewLine);
                        Directory.Delete(subDir);
                        continue;
                    }

                    /* Check each file. */
                    foreach (FileInfo file in files)
                    {
                        /* For each file (zip files), get information about its contents. */
                        using (var zipFile = File.Open(file.FullName, FileMode.Open))
                        using (var zip = new ZipArchive(zipFile, ZipArchiveMode.Update))
                        {
                            /* If the zip file is empty, then delete it. */
                            if (zip.Entries.Count < 1)
                            {
                                textBox1.AppendText("Deleting " + file.FullName + "." + Environment.NewLine);
                                zip.Dispose();
                                zipFile.Dispose();
                                File.Delete(file.FullName);
                            }
                            /* If the zip file is not empty, then check its contents. */
                            else
                            {
                                /* Check the content of each entry of the zip file. */
                                for (int i = zip.Entries.Count - 1; i >= 0; i--)
                                {
                                    using (Stream stream = zip.Entries[i].Open())
                                    {
                                        StreamReader streamReader = new StreamReader(stream);
                                        streamReader.ReadLine();

                                        /* If the entry has only a header and no data (second use of ReadLine() will return null), then delete the entry. */
                                        if (streamReader.ReadLine() == null)
                                        {
                                            textBox1.AppendText("Deleting " + zip.Entries[i].FullName + "." + Environment.NewLine);
                                            streamReader.Dispose();
                                            stream.Dispose();
                                            zip.Entries[i].Delete();
                                        }
                                    }
                                }

                                /* After checking each entry, check again if the zip file is empty, and delete it if it is the case. */
                                if (zip.Entries.Count < 1)
                                {
                                    textBox1.AppendText("Deleting " + file.FullName + "." + Environment.NewLine);
                                    zip.Dispose();
                                    zipFile.Dispose();
                                    File.Delete(file.FullName);
                                }
                            }
                        }
                    }

                    /* After checking each file, check again if the subdirectory is empty, and delete it if it is the case. */
                    subDirInfo = new DirectoryInfo(subDir);
                    files = subDirInfo.GetFiles();

                    if (files.Length < 1)
                    {
                        textBox1.AppendText("Deleting " + subDir + "." + Environment.NewLine);
                        Directory.Delete(subDir);
                    }
                }

                /* After checking all its subdirectories, check if the directory is empty, and delete it if it is the case. */
                subDirs = Directory.GetDirectories(dir);

                if (subDirs.Length < 1)
                {
                    textBox1.AppendText("Deleting " + dir + "." + Environment.NewLine);
                    Directory.Delete(dir);
                }
            }

            textBox1.AppendText("Done. DeleteEmptyFolder successfully checked all folders and subfolders contained in " + path + "." + Environment.NewLine);
        }
    }
}
