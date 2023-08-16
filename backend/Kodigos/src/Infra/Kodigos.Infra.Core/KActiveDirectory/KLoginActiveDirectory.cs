using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Text;

namespace Kodigos.Infra.Core.KActiveDirectory
{
    public class KLoginActiveDirectory
    {
        public static KRetorno<KUserModelAd> Login(KModelActiveDirectory kModelActiveDirectory)
        {
            KRetorno<KUserModelAd> retorno = new KRetorno<KUserModelAd>();
            try
            {
                DirectoryEntry directoryEntry = new DirectoryEntry(kModelActiveDirectory.Dominio,
                    kModelActiveDirectory.kUserActiveDirectory.User,
                    kModelActiveDirectory.kUserActiveDirectory.Pass);

                DirectorySearcher directorySearcher = new DirectorySearcher(directoryEntry);
                directorySearcher.Filter = "(SAMAccountName=" + kModelActiveDirectory.kUserActiveDirectory.User + ")";
                SearchResult searchResult = directorySearcher.FindOne();
                if (searchResult != null)
                {
                    retorno.Sucesso = true;
                    retorno.Message = "Usuário Autenticado!";
                    retorno.TRetorno = new KUserModelAd
                    {
                        Email = searchResult.Properties["mail"][0].ToString()
                    };
                }
                else
                {
                    retorno.Sucesso = false;
                    retorno.Message = "ERRO: Usuário/Senha Inválido!";
                }
            }
            catch (Exception e)
            {
                retorno.Sucesso = false;
                if (e.InnerException != null)
                {
                    retorno.Message = e.InnerException.ToString();
                }
                else
                {
                    retorno.Message = e.Message;
                }
            }
            return retorno;
        }
    }
}
