using BLL;

namespace wsIntegracaoXMLPadraoTrack
{
    static class Program
    {      
        static void Main()
        {
            while (true)
            {
                new Processar().ProcessarXML();
            }
        }
    }
}
