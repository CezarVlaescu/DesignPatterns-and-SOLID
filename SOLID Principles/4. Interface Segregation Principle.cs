using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SOLID_Principles
{
    // If you have a bigger interface with a lot of opperation to do, break or segragate into smaller interfaces
    // YAGNI Principle -> You Ain't Going to Need it
    internal class Interface_Segregation_Principle
    {

    }

    public class Document
    {

    }

    public interface IMachine // instand of a big interface with a lot of jobs to do, make smaller interfaces for each operation
    {
        void Print(Document document);
        void Scan(Document document);
        void Fax(Document document);
    }

    public class OldFashonPrinter : IMachine // bad approach
    {
        public void Print(Document document) { }
        public void Scan(Document document) { }
        public void Fax(Document document) { }
    }

    public interface IPrinter
    {
        void Print(Document document);
    }

    public interface IScan
    {
        void Scan(Document document);
    }

    public interface  IMultifunctionalDevice : IScan, IPrinter //.......
    {
        //...
    }

    public class MultifunctionalMachine : IMultifunctionalDevice // the good approach
    {
        private IPrinter printer;
        private IScan scanner;

        public MultifunctionalMachine(IPrinter printer, IScan scanner)
        {
            if (printer == null) 
            {
                throw new ArgumentNullException(paramName: nameof(printer)); 
            };

            if (scanner == null)
            {
                throw new ArgumentNullException(paramName: nameof(scanner));
            }

            this.printer = printer;
            this.scanner = scanner;
        }

        public void Print(Document document)
        {
            printer.Print(document);
        }

        public void Scan(Document document)
        {
            scanner.Scan(document);
        } // decorator
    }
}
