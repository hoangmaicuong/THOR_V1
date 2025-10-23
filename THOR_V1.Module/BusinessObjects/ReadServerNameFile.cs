using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THOR_V1.Module.BusinessObjects
{
    public class ReadConnectionString
    {
        public string ReadCnnStrName()
        {
            string servername, databasename, lspath, lsfullpath, str;
            string psfilename = "ServerName.Txt";
            // CryptorEngine crp = new CryptorEngine();
            StreamReader myStreamReader = null;
            lspath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            lspath = lspath.Substring(6);

            try
            {
                // Create a StreamReader using a static File class.
                lsfullpath = lspath + "\\" + psfilename;
                myStreamReader = File.OpenText(lsfullpath);
                servername = myStreamReader.ReadLine();
                databasename = myStreamReader.ReadLine();//.ReadToEnd();
                //ConstantObject.gServerMode = myStreamReader.ReadLine();
                str = "Data Source = <Server>; Initial Catalog = <Database>; User Id = th_thor; Password = WebThor@2025!;";

                str = str.Replace("<Server>", servername);
                str = str.Replace("<Database>", databasename);
                //--------------------
                // conn = CryptorEngine.Decrypt(cnstr.ToString(), true);
            }

            //-------------------
            catch (Exception exc)
            {
                // Show the exception to the user.
                return string.Empty;
            }
            finally
            {
                // Close the object if it has been created.
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                }

            }
            return str;// ;
        }
        public string ReadServerName()

        {
            string servername, databasename, lspath, lsfullpath, str;
            string psfilename = "ServerName.Txt";
            // CryptorEngine crp = new CryptorEngine();
            StreamReader myStreamReader = null;
            lspath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            lspath = lspath.Substring(6);

            try
            {
                // Create a StreamReader using a static File class.
                lsfullpath = lspath + "\\" + psfilename;
                myStreamReader = File.OpenText(lsfullpath);
                servername = myStreamReader.ReadLine();
                databasename = myStreamReader.ReadLine();

                str = servername + "-" + databasename;
                //--------------------
                // conn = CryptorEngine.Decrypt(cnstr.ToString(), true);
            }

            //-------------------
            catch (Exception exc)
            {
                // Show the exception to the user.
                return string.Empty;
            }
            finally
            {
                // Close the object if it has been created.
                if (myStreamReader != null)
                {
                    myStreamReader.Close();
                }

            }
            return str;// ;
        }
    }
}
