using com.sun.org.apache.bcel.@internal.classfile;
using com.sun.org.apache.bcel.@internal.generic;
using com.sun.tools.javac.util;
using com.sun.xml.@internal.bind.v2.model.core;
using DocumentFormat.OpenXml.Office2010.Excel;
using sun.net.idn;
using sun.reflect.generics.visitor;
using System.Net;
using System.Xml.Linq;
using VisitorSecurityClearanceSystem.Controllers;
using Xamarin.Essentials;
using static com.sun.jndi.cosnaming.IiopUrl;
using static com.sun.tools.classfile.Annotation.element_value;
using static com.sun.tools.classfile.Attribute;
using static com.sun.tools.classfile.ConstantPool;
using static com.sun.tools.classfile.ReferenceFinder;
using static com.sun.tools.classfile.StackMapTable_attribute.stack_map_frame;
using static com.sun.tools.classfile.Type;
using static com.sun.tools.@internal.xjc.model.CClassInfoParent;
using static com.sun.tools.javac.code.Attribute;
using static com.sun.tools.javac.code.Symbol;
using static com.sun.tools.javac.code.Type;
using static com.sun.tools.javac.tree.JCTree;
using static com.sun.tools.jdeps.Analyzer;
using static javax.xml.crypto.KeySelector;

namespace VisitorSecurityClearanceSystem.Models
{
    namespace VisitorSecurityClearanceSystem.Models
    {
        public class RegisterDTO
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class LoginDTO
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class VisitorDTO
        {
            // Properties for visitor
            public int id {  get; set; }
            public string Name { get; internal set; }
            public string Email { get; internal set; }
            public string Address { get; internal set; }
            public string CompanyName { get; internal set; }
            public string Purpose { get; internal set; }
            public DateTime EntryTime { get; internal set; }
            public DateTime ExitTime { get; internal set; }
           
        }
        public class VisitorEntity(VisitorDTO visitor)
        {
            public string Id { get; private set; }
            public string Name { get; }
            public string Email { get; }
            public string Address { get; }

            public string CompanyName { get; }
            public string Purpose { get; }
            public DateTime EntryTime { get; }
            public DateTime ExitTime { get; }

            // Assign DTO properties to entity properties
           

            
            // Assign other properties as needed
        }


        public class SecurityDTO
        {
            public string Name { get; set; }
            public object Role { get; internal set; }

            public string Email { get; set; }
            public string id { get; internal set; }

            // Properties for security personnel
        }


        public class ManagerDTO
        {
            // Properties for manager
            public object Name {  get; set; }
            public object Department { get; internal set; }
            public string Email { get; internal set; }
        }

        public class OfficeDTO
        {
            // Properties for office

            public object Name { get; set; }
            public string Location { get; internal set; }
        }

        public class PassDTO
        {
            
            // Properties for pass

        }

        public class EmailDTO
        {
            public string SenderEmail { get; set; }
            public List<string> RecipientEmails { get; set; }
            public string Subject { get; set; }
            public string Content { get; set; }
        }
    }

}
