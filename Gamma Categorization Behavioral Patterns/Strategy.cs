using DocumentFormat.OpenXml.Office2010.PowerPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gamma_Categorization_Behavioral_Patterns
{
    // Many algorithms can be decomposed into higher and lower level parts
    // Making tea can be decomposed into, processs of making a hot beverage, tea-specific things
    // The high level algo can then be reused for making coffee or hot chocolate
    // def -> enables the exact behavior of a system to be selected either at run-time ( dynamic ) or compile-time ( static ). It's called policy in c++
    // Define an algorithm at high level, define the interface you expect each strategy to follow, provide for either dynamic or static composition of strategy in the overall algorithm

    // dynamic strategy

    public enum OutputFormat
    {
        Markdown,
        Html
    }

    public interface IListStrategy
    {
        void Start(StringBuilder sb);
        void End(StringBuilder sb);
        void AddListItem(StringBuilder sb, string value);
    }

    public class HtmlStrategy : IListStrategy
    {
        void IListStrategy.AddListItem(StringBuilder sb, string value)
        {
            sb.Append($"<li>{value}</li>");
        }

        void IListStrategy.End(StringBuilder sb)
        {
            sb.AppendLine("</ul>");
        }

        void IListStrategy.Start(StringBuilder sb)
        {
            sb.AppendLine("<ul>");
        }
    }

    public class MarkdownStrategy : IListStrategy
    {
        void IListStrategy.AddListItem(StringBuilder sb, string value)
        {
            sb.Append($" * {value}");
        }

        void IListStrategy.End(StringBuilder sb)
        {
            throw new NotImplementedException();
        }

        void IListStrategy.Start(StringBuilder sb)
        {
            throw new NotImplementedException();
        }
    }

    internal class TextProcessor
    {
        private StringBuilder sb = new StringBuilder();
        private IListStrategy listStrategy;

        public void SetOutputFormat(OutputFormat outputFormat)
        {
            switch (outputFormat)
            {
                case OutputFormat.Markdown:
                    listStrategy = new MarkdownStrategy();
                    break;
                case OutputFormat.Html: 
                    listStrategy = new HtmlStrategy(); 
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void AppendList(IEnumerable<string> list)
        {
            listStrategy.Start(sb);
            foreach (string item in list)
            {
                listStrategy.AddListItem (sb, item);
            }
            listStrategy.End(sb);
        }

        public StringBuilder Clear()
        {
            return sb.Clear();
        }

        public override string ToString() 
        {
            return sb.ToString();
        }
    }

    // static strategy

    internal class TextProcessor2<LS> where LS : IListStrategy, new() 
    {
        private StringBuilder sb = new StringBuilder();
        private IListStrategy listStrategy = new LS();

        public void SetOutputFormat(OutputFormat outputFormat)
        {
            switch (outputFormat)
            {
                case OutputFormat.Markdown:
                    listStrategy = new MarkdownStrategy();
                    break;
                case OutputFormat.Html:
                    listStrategy = new HtmlStrategy();
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        public void AppendList(IEnumerable<string> list)
        {
            listStrategy.Start(sb);
            foreach (string item in list)
            {
                listStrategy.AddListItem(sb, item);
            }
            listStrategy.End(sb);
        }

        public StringBuilder Clear()
        {
            return sb.Clear();
        }

        public override string ToString()
        {
            return sb.ToString();
        }
    }

    internal class Demoing
    {
        static void Main(string[] args)
        {
            var tp = new TextProcessor();
            tp.SetOutputFormat(OutputFormat.Markdown);
            tp.AppendList(new[] {"foo", "bar", "baz"}); 
            Console.WriteLine(tp);
        }
    }
    
}
