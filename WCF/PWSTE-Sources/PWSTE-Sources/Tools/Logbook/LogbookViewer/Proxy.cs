// © 2011 IDesign Inc. 
//Questions? Comments? go to 
//http://www.idesign.net

using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Runtime.Serialization;
using ServiceModelEx;

namespace ServiceModelEx
{
   [DataContract]
   struct LogbookEntry
   {
      string AssemblyNameField;
      string DateField;
      string EventField;
      string ExceptionMessageField;
      string ExceptionNameField;
      string FileNameField;
      string HostNameField;
      int LineNumberField;
      string MachineNameField;
      string MemberAccessedField;
      string TimeField;
      string TypeNameField;

      [DataMember]
      public string AssemblyName
      {
         get
         {
            return AssemblyNameField;
         }
         set
         {
            AssemblyNameField = value;
         }
      }

      [DataMember]
      public string Date
      {
         get
         {
            return DateField;
         }
         set
         {
            DateField = value;
         }
      }

      [DataMember]
      public string Event
      {
         get
         {
            return EventField;
         }
         set
         {
            EventField = value;
         }
      }

      [DataMember]
      public string ExceptionMessage
      {
         get
         {
            return ExceptionMessageField;
         }
         set
         {
            ExceptionMessageField = value;
         }
      }

      [DataMember]
      public string ExceptionName
      {
         get
         {
            return ExceptionNameField;
         }
         set
         {
            ExceptionNameField = value;
         }
      }

      [DataMember]
      public string FileName
      {
         get
         {
            return FileNameField;
         }
         set
         {
            FileNameField = value;
         }
      }

      [DataMember]
      public string HostName
      {
         get
         {
            return HostNameField;
         }
         set
         {
            HostNameField = value;
         }
      }

      [DataMember]
      public int LineNumber
      {
         get
         {
            return LineNumberField;
         }
         set
         {
            LineNumberField = value;
         }
      }

      [DataMember]
      public string MachineName
      {
         get
         {
            return MachineNameField;
         }
         set
         {
            MachineNameField = value;
         }
      }

      [DataMember]
      public string MemberAccessed
      {
         get
         {
            return MemberAccessedField;
         }
         set
         {
            MemberAccessedField = value;
         }
      }
      [DataMember]
      public string Time
      {
         get
         {
            return TimeField;
         }
         set
         {
            TimeField = value;
         }
      }

      [DataMember]
      public string TypeName
      {
         get
         {
            return TypeNameField;
         }
         set
         {
            TypeNameField = value;
         }
      }
   }
}

[ServiceContract]
interface ILogbookManager
{
   [OperationContract(IsOneWay = true)]
   void LogEntry(LogbookEntry entry);

   [OperationContract]
   LogbookEntry[] GetEntries();

   [OperationContract(IsOneWay = true)]
   void Clear();
}


class LogbookManagerClient : ClientBase<ILogbookManager>,ILogbookManager
{
   public LogbookManagerClient()
   {}

   public LogbookManagerClient(string endpointConfigurationName) : base(endpointConfigurationName)
   {}

   public LogbookManagerClient(string endpointConfigurationName,string remoteAddress) : base(endpointConfigurationName,remoteAddress)
   {}

   public LogbookManagerClient(Binding binding,EndpointAddress remoteAddress) : base(binding,remoteAddress)
   {}

   public void LogEntry(LogbookEntry entry)
   {
      Channel.LogEntry(entry);
   }

   public LogbookEntry[] GetEntries()
   {
      return Channel.GetEntries();
   }

   public void Clear()
   {
      Channel.Clear();
   }
}
