using AE.Net.Mail;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Services;

namespace SondaCorreo
{
    public partial class LecturaCorreo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        [WebMethod]
        public static string Process()
        {

            MailMessage messages = new MailMessage();
            string[] body;

            using (var cliente = new ImapClient("imap.gmail.com", "javier.escobarb.etb@gmail.com", "T0m4s1t0", AuthMethods.Login, 993, true))
            {
                var msj = cliente.SearchMessages(SearchCondition.Unseen()).Where(x => x.Value.Subject.Contains("LEADS")).Where(x => x.Value.Subject.Contains(":"));

                foreach (var id in msj.Select(x => x.Value.Uid))
                {
                    try
                    {
                        string Nombre = string.Empty;
                        string Correo = string.Empty;
                        string Telefono1 = string.Empty;
                        string Telefono2 = string.Empty;

                        messages = cliente.GetMessage(id, false, false);

                        string[] salto = new string[] { "\r\n" };
                        body = messages.Body.Replace("Nombre", "").Replace("Correo electrónico", "").Replace("Teléfono 1", "").Replace("Teléfono 2", "").Replace(": ", ":").Replace("________________________________", "").Split(salto, StringSplitOptions.None);

                        for (int i = 0; i < body.Length; i++)
                        {
                            if (body[i].Contains(":"))
                            {
                                if (Nombre == string.Empty)
                                    Nombre = body[i].Replace(":", "");
                                else if (Correo == string.Empty)
                                    Correo = body[i].Replace(":", "");
                                else if (Telefono1 == string.Empty)
                                    Telefono1 = body[i].Replace(":", "");
                                else if (Telefono2 == string.Empty)
                                {
                                    Telefono2 = body[i].Replace(":", "");
                                    break;
                                }
                            }
                        }

                        Dictionary<string, object> Parms = new Dictionary<string, object>();

                        Parms.Add("Nombre", Nombre);
                        Parms.Add("Correo", Correo);
                        Parms.Add("Telefono1", Telefono1);
                        Parms.Add("Telefono2", Telefono2);
                        Parms.Add("Origen", "LeadCorreo");

                        if (messages.Headers["From"].Value.Contains("automate"))
                            Parms.Add("Fuente", "facebook");
                        else if (messages.Headers["From"].Value.Contains("mccclictag"))
                            Parms.Add("Fuente", "clictag");

                        Parms.Add("Medio", "CPL");

                        char[] array = messages.Subject.ToCharArray();
                        string camp = "";
                        bool campana = false;

                        for (int i = 0; i < array.Length; i++)
                        {
                            if (campana)
                                camp += array[i];

                            if (array[i] == ':' && array[i + 1] != ' ')
                            {
                                campana = true;
                                if (array[i + 1] == 'h' && array[i + 2] == '_')
                                    Parms.Add("Oferta", "Hogares");
                                else if (array[i + 1] == 'n' && array[i + 2] == '_')
                                    Parms.Add("Oferta", "Negocios");
                                else if (array[i + 1] == 'm' && array[i + 2] == '_')
                                    Parms.Add("Oferta", "Móviles");
                                else if (array[i + 1] == 'p' && array[i + 2] == '_')
                                    Parms.Add("Oferta", "Planta");
                            }
                            else if (!Parms.ContainsKey("Oferta") && array[i + 1] == ' ')
                            {
                                if (array[i + 2] == 'h' && array[i + 3] == '_')
                                    Parms.Add("Oferta", "Hogares");
                                else if (array[i + 2] == 'n' && array[i + 3] == '_')
                                    Parms.Add("Oferta", "Negocios");
                                else if (array[i + 2] == 'm' && array[i + 3] == '_')
                                    Parms.Add("Oferta", "Móviles");
                                else if (array[i + 2] == 'p' && array[i + 3] == '_')
                                    Parms.Add("Oferta", "Planta");

                                if (Parms.ContainsKey("Oferta"))
                                    campana = true;
                            }
                        }

                        if (!Parms.ContainsKey("Oferta"))
                            Parms.Add("Oferta", "Otros");

                        Parms.Add("Campaña", camp);

                        WS_Resources.Resources res = new WS_Resources.Resources();

                        string resultado = res.InsertMail_Agosto(Parms["Nombre"].ToString(), Parms["Correo"].ToString(), Parms["Telefono1"].ToString(), Parms["Telefono2"].ToString(), Parms["Origen"].ToString(), Parms["Fuente"].ToString(), Parms["Medio"].ToString(), Parms["Oferta"].ToString(), Parms["Campaña"].ToString());

                        cliente.MoveMessage(id, "ArchivadosLeader");
                    }
                    catch (Exception ex)
                    {
                        //return ex.Message;
                    }
                }

                return "TODO BIEN";
            }
        }
    }
}