using System;
using System.IO;
using System.Xml;
using MonkeyPants.Output.Channels;
using MonkeyPants.Results;

namespace MonkeyPants.Output
{
    public class HtmlFileResultsWriter : IResultsWriter
    {
        private readonly IOutputChannel channel;
        private XmlWriter xmlWriter;

        public HtmlFileResultsWriter(IOutputChannel channel)
        {
            this.channel = channel;
        }

        public void Open()
        {
            channel.Open(Accept);

            xmlWriter.WriteStartElement("html");
            xmlWriter.WriteStartElement("head");
            
            xmlWriter.WriteStartElement("style");
            xmlWriter.WriteAttributeString("type", "text/css");
            xmlWriter.WriteRaw(CSS);
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();

            xmlWriter.WriteStartElement("body");
        }

        private void Accept(TextWriter textWriter)
        {            
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;                        
            xmlWriter = XmlWriter.Create(textWriter, settings);
        }

        public void Close()
        {
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();
            channel.Close();
        }

        public void Begin(SuiteResult suite)
        {
            StartSpan("suite");
            xmlWriter.WriteRaw("Suite: " + suite.SuiteName);
            EndCurrentElement();
        }

        public void End(SuiteResult suite)
        {
            EndCurrentElement();
        }

        public void Begin(ScenarioResult scenario)
        {
            StartSpan("scenario");
            xmlWriter.WriteRaw("Scenario: " + scenario.ScenarioName);
            EndCurrentElement();
        }

        public void End(ScenarioResult scenario)
        {
            // already ended
        }

        // todo: expandable nested table
        public void Begin(TestSetupResult test)
        {
            xmlWriter.WriteRaw("YO YO YO YO starting setup for " + test);
        }

        // todo: expandable nested table
        public void End(TestSetupResult test)
        {
            xmlWriter.WriteRaw("YO YO YO YO end of setup for " + test);
        }

        public void Begin(TestResult test)
        {
            xmlWriter.WriteStartElement("table");

            xmlWriter.WriteStartElement("tr");
            xmlWriter.WriteStartElement("td");

            StartSpan("fixture");
            xmlWriter.WriteRaw(test.TestName);
            EndCurrentElement(); // fixture
            
            EndCurrentElement(); // td
            EndCurrentElement(); // tr

            xmlWriter.WriteStartElement("tr");
            test.Headers.ForEach(s => xmlWriter.WriteElementString("td", s));
            EndCurrentElement(); //tr
        }

        public void End(TestResult test)
        {
            EndCurrentElement();
        }

        public void Begin(RowResult row)
        {
            xmlWriter.WriteStartElement("tr");
        }

        public void End(RowResult row)
        {
            EndCurrentElement();
        }
        
        public void Begin(CellResult cell)
        {
            string @class = "pass";
            if (cell.Passed)
            {
                @class = "pass";
            }
            else if (cell.Failed)
            {
                @class = "fail";
            }
            else if (cell.Errored)
            {
                @class = "error";
            }
            xmlWriter.WriteStartElement("td");
            xmlWriter.WriteAttributeString("class", @class);
        }

        public void End(CellResult cell)
        {            
            if (Equals(cell.ExpectedResult, cell.ActualResult))
            {
                xmlWriter.WriteRaw(cell.ActualResult);                
            }
            else if (cell.Exception == null)
            {
                xmlWriter.WriteRaw(string.Format("Expected:<br/>{0}<hr/ >Actual:<br/>{1}", cell.ExpectedResult, cell.ActualResult));
            }
            else
            {
                xmlWriter.WriteRaw("Exception: " + cell.Exception.Message);
                WriteBr();
                xmlWriter.WriteRaw("Stack trace: " + cell.Exception.StackTrace.Replace(Environment.NewLine, "<br/>"));
            }
            xmlWriter.WriteEndElement();
        }

        private void StartSpan(string @class)
        {
            xmlWriter.WriteStartElement("span");
            xmlWriter.WriteAttributeString("class", @class);
        }

        private void EndCurrentElement()
        {
            xmlWriter.WriteEndElement();
        }

        private void WriteBr()
        {
            xmlWriter.WriteStartElement("br");
            xmlWriter.WriteEndElement();
        }

        const string CSS = @"
/**********

 Global effects

**********/

body
{
	margin: 5px;
	padding: 0px;
	font-family: trebuchet ms, verdana, tahoma, arial, sans-serif;
	font-size: 0.7em;
	background-color: white;
}

a:link
{
	color: #2E2EB2;
	background: transparent;
}

a:visited
{
	color: #606099;
	background: transparent;
}

a:hover
{
	color: #8989D9;
	background: transparent;
	text-decoration: none;
}

a:active
{
	color: #FF8000;
	background: transparent;
}

table
{
	background: #EEEEEE;
	margin: 10px;
	padding: 0px;
	border: solid #AAAAAA;
	border-width: 1px 0px 0px 1px;
	font-size: 0.9em;
}

td, th
{
	border: solid #AAAAAA;
	border-width: 0px 1px 1px 0px;
	padding: 1px 3px 1px 3px;
}

h1
{
	font-size: 3.0em;
	font-variant: small-caps;
	margin: 0.5em
}

h2
{
	font-size: 2.2em;
	font-variant: small-caps;
	margin: 0.5em
}

h3
{
	font-size: 1.7em;
	font-variant: small-caps;
	margin: 0.5em
}

textarea
{
	width: 100%;
}

pre
{
	font-size: 1.5em;
}

img
{
	border: 0px;
}

/**********

 Structural

**********/

div.sidebar
{
	margin: 0px;
	padding: 5px;
	position: fixed;
	top: 0px;
	left: 0px;
	width: 95px;
	height: 1000px;
	border-right: dotted 1px #FF8000;
}

div.actions
{
	margin: 0px;
	padding: 0px;
	border-top: solid 3px white;
}

div.mainbar
{
	margin: 0px;
	position: absolute;
	top: 0px;
	left: 110px;
	width: 87%;
}

div.header
{
	margin: 0px;
	padding: 5px;
	height: 90px;
	border-bottom: solid 1px #FF8000;
}

div.main
{
	margin: 0px;
	padding: 5px;
}

#execution-status
{
	position: absolute;
	top: 0px;
	right: 0px;
	width: 100px;
	height: 90px;
	margin: 5px;
	text-align: center;
}

#test-summary
{
	width: 99%;
	top: 105px;
	padding: 2px 2px 2px 5px;
	margin: 0px 0px 5px 0px;
	font-size: 1.2em;
	border: solid black 1px;
}

.actions a:link, .actions a:visited
{
	display: block;
	margin: 0px;
	line-height: 2.2em;
	text-indent: 1em;
	color: #FFF;
	font-size: 0.8em;
	font-weight: bold;
	background-color: #2E2EB2;
	text-decoration: none;
	border: solid 1px white;
	border-width: 0px 0px 2px 0px;
}

.actions a:hover
{
	color: #FF8000;
	background-color: #606099;
}

.header a:link
{
	font-weight: bold;
}

/**********

 Miscellaneous formatting

**********/

.note
{
	font-size: 0.75em;
	color: #999999;
}

.page_title
{
	font-size: 3.0em;
}

span.page_title
{
	text-shadow: 0px 0px 4px #2E2EB2;
}

.page_type
{
	font-size: 1.7em;
	font-variant: small-caps;
}

.caps, .resultsHeader
{
	font-size: 1.3em;
	font-variant: small-caps;
}

.resultsHeader, .resultsRow2
{
	background-color: #EFEFEF;
}

.resultsRow1
{
	background-color: #FFFFFF;
}

.centered
{
	margin: 5px;
	text-align: center;
}

.right
{
	float: right;
}

.left
{
	float: left;
}

.strike
{
	text-decoration: line-through;
}

.meta
{
	color: #BF8660;
	font-style: italic;
}

.included, .setup, .teardown, .collapse_rim
{
	background: #F9F9F9;
	margin: 5px 0px 5px 0px;
	padding: 2px 2px 2px 2px;
	border: 1px dotted #909090;
}

.setup, .teardown
{
	background: #FFFFF0;
	border-color: #FF8000;
}

.included
{
	background: #F9F9FF;
	border-color: #2E2EB2;
}

.collapse_rim
{
	border-color: #BF8660;
}

.alternating_row_1, .alternating_row_2
{
	display: block;
	height: 1.5em;
	width: 100%;
}

.alternating_row_1, .alternating_block_1
{
	background-color: #EFEFEF;
}

.alternating_row_2, .alternating_block_2
{
	background-color: #FFFFFF;
}

.alternating_block_1, .alternating_block_2
{
	border: solid 1px #DFD4A7;
}

div.collapsable
{
	margin: 0px 0px 0px 15px;
	display: block;
}

.hidden
{
	display: none;
}

.toc1 ul
{
	list-style-type: disc;
	padding-left: 2em;
	color: black;
}

.toc2 ul
{
	list-style-type: circle;
}

.toc3 ul
{
	list-style-type: disc;
	color: gray;
}

.toc4 ul, .toc5 ul, .toc6 ul
{
	list-style-type: circle;
}

/**********

 Page Specific

**********/

div.edit_buttons
{
}

div.nav_break
{
	font-size: .3em;
}

div.merge_new
{
	float: left;
	font-size: 1.4em;
	width: 49%;
}

div.merge_old
{
	float: right;
	font-size: 1.4em;
	width: 49%;
}

.test_summary_link
{
	text-align: right;
}

.test_summary_results
{
	padding: 0px 5px 0px 5px;
	text-align: left;
}

.suite_summary
{
	margin: 5px;
	margin-left: auto;
	margin-right: auto;
	text-align: center;
}

.test_output_name
{
	margin-top: 10px;
	background-color: #DFD4A7;
	font-size: 1.5em;
	text-indent: 1em;
}

table.dirListing
{
	background: transparent;
	margin: 2px;
	padding: 0px;
	border: 0px;
	width: 99%;
	font-size: 1.0em;
}

.dirListing td, .dirListing th
{
	border: 0px;
	padding: 1px 3px 1px 3px;
}

/**********

 Used in FIT

**********/

.pass
{
	background-color: #AAFFAA;
}

.fail
{
	background-color: #FFAAAA;
}

.error
{
	background-color: #FFFFAA;
}

.ignore
{
	background-color: #CCCCCC;
}

.fit_stacktrace
{
	font-size: 0.7em;
}

.fit_label
{
	font-style: italic;
	color: #C08080;
}

.fit_grey
{
	color: #808080;
}

.TestName
    {
        font-weight: bold;
        font-size: 2em;
    }
";

    }
}
