using log4net;
using PoorMansTSqlFormatterLib.Formatters;
using PoorMansTSqlFormatterLib.Interfaces;
using PoorMansTSqlFormatterLib.Parsers;
using PoorMansTSqlFormatterLib.Tokenizers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace ClipboardSqlFormatter
{
    public class SqlFormatter
    {
        #region Регистратор
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        public bool FormatSql(string inputText, out string outputText)
        {
            if (!CheckTextStartsWithExecCommand(inputText))
            {
                outputText = null;
                return false;
            }

            try
            {
                var tree = GetSqlTree(MinificateSql(inputText));
                var root = XElement.Parse(tree.OuterXml);

                var sqlClause = root.Elements(SqlXmlConstants.ENAME_SQL_STATEMENT).Single().Elements(SqlXmlConstants.ENAME_SQL_CLAUSE).Single();

                var parameters = GetParameters(sqlClause.Elements(SqlXmlConstants.ENAME_COMMA).ElementAt(1)).ToDictionary(p => p.Key, p => p.Value);

                var sqlQuery = sqlClause.Elements().First(n => n.Name == SqlXmlConstants.ENAME_NSTRING || n.Name == SqlXmlConstants.ENAME_STRING);

                string sqlWithParameters = SubstituteParameters(sqlQuery.Value, parameters);

                outputText = FormatSql(sqlWithParameters);
                return true;
            }
            catch (Exception e)
            {
                outputText = null;
                _log.Error(string.Format("FormatSql: an error occured while formatting following string: \n{0}", inputText), e);
                return false;
            }
        }

        private string FormatSql(string text)
        {
            return new TSqlStandardFormatter().FormatSQLTree(
                        new TSqlStandardParser().ParseSQL(
                            new TSqlStandardTokenizer().TokenizeSQL(text)));
        }

        private IEnumerable<KeyValuePair<string, string>> GetParameters(XElement searchFrom)
        {
            XElement parName;
            while ((parName = searchFrom.ElementsAfterSelf(SqlXmlConstants.ENAME_OTHERNODE).FirstOrDefault()) != null)
            {
                var parValue = parName.ElementsAfterSelf().First(n => n.Name != SqlXmlConstants.ENAME_EQUALSSIGN && n.Name != SqlXmlConstants.ENAME_WHITESPACE);
                yield return new KeyValuePair<string, string>(parName.Value, Format(parValue));

                searchFrom = parValue;
            }
        }

        private XmlDocument GetSqlTree(string text)
        {
            return new TSqlStandardParser().ParseSQL(
                        new TSqlStandardTokenizer().TokenizeSQL(text));
        }

        private string MinificateSql(string inputText)
        {
            return new TSqlObfuscatingFormatter().FormatSQLTree(
                        new TSqlStandardParser().ParseSQL(
                            new TSqlStandardTokenizer().TokenizeSQL(inputText)));
        }

        private bool CheckTextStartsWithExecCommand(string text)
        {
            const string execSqlString = "exec sp_executesql";
            int firstSymbolIndex = 0;

            var spaces = new[] { ' ', '\t', '\n', '\r' };
            while (firstSymbolIndex < text.Length && spaces.Contains(text[firstSymbolIndex]))
            {
                firstSymbolIndex++;
            }

            return text.Length > execSqlString.Length + firstSymbolIndex && text.Substring(firstSymbolIndex, execSqlString.Length) == execSqlString;
        }

        private string SubstituteParameters(string sqlQuery, Dictionary<string, string> parameters)
        {
            foreach (var parameter in parameters)
            {
                sqlQuery = sqlQuery.Replace(parameter.Key, parameter.Value);
            }
            return sqlQuery;
        }

        private string Format(XElement node)
        {
            var result = node.Value;

            if (node.Name == SqlXmlConstants.ENAME_STRING)
                result = String.Format("'{0}'", result);

            if (node.Name == SqlXmlConstants.ENAME_NSTRING)
                result = String.Format("N'{0}'", result);

            return result;
        }
    }
}
