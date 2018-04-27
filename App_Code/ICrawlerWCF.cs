using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

// ПРИМЕЧАНИЕ. Можно использовать команду "Переименовать" в меню "Рефакторинг", чтобы изменить имя интерфейса "ICrawlerWCF" в коде и файле конфигурации.

[ServiceContract]
public interface ICrawlerWCF
{
    [OperationContract]
    byte[] GetContent(string Url, int mapid, bool a_replace);

    [OperationContract]
    string Lemmas(string Url, string[] pr);
}

