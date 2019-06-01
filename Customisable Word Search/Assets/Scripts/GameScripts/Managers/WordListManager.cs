using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using HtmlAgilityPack;
using System;
using LemmaSharp;

namespace WordGenerator
{
	public class WordListManager : MonoBehaviour
    {
        private readonly static string[] stopwords = new string[] { "a", "about", "above", "after", "again", "against", "ain", "all", "am", "an", "and", "any", "are", "aren",
        "aren't", "as", "at", "be", "because", "been", "before", "being", "below", "between", "both", "but", "by", "can", "couldn", "couldn't", "d", "did", "didn", "didn't", "do", "does", "doesn", "doesn't", "doing", "don", "don't", "down", "during", "each", "few", "for", "from", "further", "had", "hadn", "hadn't", "has", "hasn", "hasn't", "have", "haven", "haven't", "having", "he", "her", "here", "hers", "herself", "him", "himself", "his", "how", "i", "if", "in", "into", "is", "isn", "isn't", "it", "it's", "its", "itself", "just", "ll", "m", "ma", "me", "mightn", "mightn't", "more", "most", "mustn", "mustn't", "my", "myself", "needn", "needn't", "no", "nor", "not", "now", "o", "of", "off", "on", "once", "only", "or", "other", "our", "ours", "ourselves", "out", "over", "own", "re", "s", "same", "shan", "shan't", "she", "she's", "should", "should've", "shouldn", "shouldn't", "so", "some", "such", "t", "than", "that", "that'll", "the", "their", "theirs", "them", "themselves", "then", "there", "these", "they", "this", "those", "through", "to", "too", "under", "until", "up", "ve", "very", "was", "wasn", "wasn't", "we", "were", "weren", "weren't", "what", "when", "where", "which", "while", "who", "whom", "why", "will", "with", "won", "won't", "wouldn", "wouldn't", "y", "you", "you'd", "you'll", "you're", "you've", "your", "yours", "yourself", "yourselves", "could", "he'd", "he'll", "he's", "here's", "how's", "i'd", "i'll", "i'm", "i've", "let's", "ought", "she'd", "she'll", "that's", "there's", "they'd", "they'll", "they're", "they've", "we'd", "we'll", "we're", "we've", "what's", "when's", "where's", "who's", "why's", "would", "able", "abst", "accordance", "according", "accordingly", "across", "act", "actually", "added", "adj", "affected", "affecting", "affects", "afterwards", "ah", "almost", "alone", "along", "already", "also", "although", "always", "among", "amongst", "announce", "another", "anybody", "anyhow", "anymore", "anyone", "anything", "anyway", "anyways", "anywhere", "apparently", "approximately", "arent", "arise", "around", "aside", "ask", "asking", "auth", "available", "away", "awfully", "b", "back", "became", "become", "becomes", "becoming", "beforehand", "begin", "beginning", "beginnings", "begins", "behind", "believe", "beside", "besides", "beyond", "biol", "brief", "briefly", "c", "ca", "came", "cannot", "can't", "cause", "causes", "certain", "certainly", "co", "com", "come", "comes", "contain", "containing", "contains", "couldnt", "date", "different", "done", "downwards", "due", "e", "ed", "edu", "effect", "eg", "eight", "eighty", "either", "else", "elsewhere", "end", "ending", "enough", "especially", "et", "etc", "even", "ever", "every", "everybody", "everyone", "everything", "everywhere", "ex", "except", "f", "far", "ff", "fifth", "first", "five", "fix", "followed", "following", "follows", "former", "formerly", "forth", "found", "four", "furthermore", "g", "gave", "get", "gets", "getting", "give", "given", "gives", "giving", "go", "goes", "gone", "got", "gotten", "h", "happens", "hardly", "hed", "hence", "hereafter", "hereby", "herein", "heres", "hereupon", "hes", "hi", "hid", "hither", "home", "howbeit", "however", "hundred", "id", "ie", "im", "immediate", "immediately", "importance", "important", "inc", "indeed", "index", "information", "instead", "invention", "inward", "itd", "it'll", "j", "k", "keep", "keeps", "kept", "kg", "km", "know", "known", "knows", "l", "largely", "last", "lately", "later", "latter", "latterly", "least", "less", "lest", "let", "lets", "like", "liked", "likely", "line", "little", "'ll", "look", "looking", "looks", "ltd", "made", "mainly", "make", "makes", "many", "may", "maybe", "mean", "means", "meantime", "meanwhile", "merely", "mg", "might", "million", "miss", "ml", "moreover", "mostly", "mr", "mrs", "much", "mug", "must", "n", "na", "name", "namely", "nay", "nd", "near", "nearly", "necessarily", "necessary", "need", "needs", "neither", "never", "nevertheless", "new", "next", "nine", "ninety", "nobody", "non", "none", "nonetheless", "noone", "normally", "nos", "noted", "nothing", "nowhere", "obtain", "obtained", "obviously", "often", "oh", "ok", "okay", "old", "omitted", "one", "ones", "onto", "ord", "others", "otherwise", "outside", "overall", "owing", "p", "page", "pages", "part", "particular", "particularly", "past", "per", "perhaps", "placed", "please", "plus", "poorly", "possible", "possibly", "potentially", "pp", "predominantly", "present", "previously", "primarily", "probably", "promptly", "proud", "provides", "put", "q", "que", "quickly", "quite", "qv", "r", "ran", "rather", "rd", "readily", "really", "recent", "recently", "ref", "refs", "regarding", "regardless", "regards", "related", "relatively", "research", "respectively", "resulted", "resulting", "results", "right", "run", "said", "saw", "say", "saying", "says", "sec", "section", "see", "seeing", "seem", "seemed", "seeming", "seems", "seen", "self", "selves", "sent", "seven", "several", "shall", "shed", "shes", "show", "showed", "shown", "showns", "shows", "significant", "significantly", "similar", "similarly", "since", "six", "slightly", "somebody", "somehow", "someone", "somethan", "something", "sometime", "sometimes", "somewhat", "somewhere", "soon", "sorry", "specifically", "specified", "specify", "specifying", "still", "stop", "strongly", "sub", "substantially", "successfully", "sufficiently", "suggest", "sup", "sure", "take", "taken", "taking", "tell", "tends", "th", "thank", "thanks", "thanx", "thats", "that've", "thence", "thereafter", "thereby", "thered", "therefore", "therein", "there'll", "thereof", "therere", "theres", "thereto", "thereupon", "there've", "theyd", "theyre", "think", "thou", "though", "thoughh", "thousand", "throug", "throughout", "thru", "thus", "til", "tip", "together", "took", "toward", "towards", "tried", "tries", "truly", "try", "trying", "ts", "twice", "two", "u", "un", "unfortunately", "unless", "unlike", "unlikely", "unto", "upon", "ups", "us", "use", "used", "useful", "usefully", "usefulness", "uses", "using", "usually", "v", "value", "various", "'ve", "via", "viz", "vol", "vols", "vs", "w", "want", "wants", "wasnt", "way", "wed", "welcome", "went", "werent", "whatever", "what'll", "whats", "whence", "whenever", "whereafter", "whereas", "whereby", "wherein", "wheres", "whereupon", "wherever", "whether", "whim", "whither", "whod", "whoever", "whole", "who'll", "whomever", "whos", "whose", "widely", "willing", "wish", "within", "without", "wont", "words", "world", "wouldnt", "www", "x", "yes", "yet", "youd", "youre", "z", "zero", "a's", "ain't", "allow", "allows", "apart", "appear", "appreciate", "appropriate", "associated", "best", "better", "c'mon", "c's", "cant", "changes", "clearly", "concerning", "consequently", "consider", "considering", "corresponding", "course", "currently", "definitely", "described", "despite", "entirely", "exactly", "example", "going", "greetings", "hello", "help", "hopefully", "ignored", "inasmuch", "indicate", "indicated", "indicates", "inner", "insofar", "it'd", "keep", "keeps", "novel", "presumably", "reasonably", "second", "secondly", "sensible", "serious", "seriously", "sure", "t's", "third", "thorough", "thoroughly", "three", "well", "wonder", "a", "about", "above", "above", "across", "after", "afterwards", "again", "against", "all", "almost", "alone", "along", "already", "also", "although", "always", "am", "among", "amongst", "amoungst", "amount", "an", "and", "another", "any", "anyhow", "anyone", "anything", "anyway", "anywhere", "are", "around", "as", "at", "back", "be", "became", "because", "become", "becomes", "becoming", "been", "before", "beforehand", "behind", "being", "below", "beside", "besides", "between", "beyond", "bill", "both", "bottom", "but", "by", "call", "can", "cannot", "cant", "co", "con", "could", "couldnt", "cry", "de", "describe", "detail", "do", "done", "down", "due", "during", "each", "eg", "eight", "either", "eleven", "else", "elsewhere", "empty", "enough", "etc", "even", "ever", "every", "everyone", "everything", "everywhere", "except", "few", "fifteen", "fify", "fill", "find", "fire", "first", "five", "for", "former", "formerly", "forty", "found", "four", "from", "front", "full", "further", "get", "give", "go", "had", "has", "hasnt", "have", "he", "hence", "her", "here", "hereafter", "hereby", "herein", "hereupon", "hers", "herself", "him", "himself", "his", "how", "however", "hundred", "ie", "if", "in", "inc", "indeed", "interest", "into", "is", "it", "its", "itself", "keep", "last", "latter", "latterly", "least", "less", "ltd", "made", "many", "may", "me", "meanwhile", "might", "mill", "mine", "more", "moreover", "most", "mostly", "move", "much", "must", "my", "myself", "name", "namely", "neither", "never", "nevertheless", "next", "nine", "no", "nobody", "none", "noone", "nor", "not", "nothing", "now", "nowhere", "of", "off", "often", "on", "once", "one", "only", "onto", "or", "other", "others", "otherwise", "our", "ours", "ourselves", "out", "over", "own", "part", "per", "perhaps", "please", "put", "rather", "re", "same", "see", "seem", "seemed", "seeming", "seems", "serious", "several", "she", "should", "show", "side", "since", "sincere", "six", "sixty", "so", "some", "somehow", "someone", "something", "sometime", "sometimes", "somewhere", "still", "such", "system", "take", "ten", "than", "that", "the", "their", "them", "themselves", "then", "thence", "there", "thereafter", "thereby", "therefore", "therein", "thereupon", "these", "they", "thickv", "thin", "third", "this", "those", "though", "three", "through", "throughout", "thru", "thus", "to", "together", "too", "top", "toward", "towards", "twelve", "twenty", "two", "un", "under", "until", "up", "upon", "us", "very", "via", "was", "we", "well", "were", "what", "whatever", "when", "whence", "whenever", "where", "whereafter", "whereas", "whereby", "wherein", "whereupon", "wherever", "whether", "which", "while", "whither", "who", "whoever", "whole", "whom", "whose", "why", "will", "with", "within", "without", "would", "yet", "you", "your", "yours", "yourself", "yourselves", "the", "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "co", "op", "research-articl", "pagecount", "cit", "ibid", "les", "le", "au", "que", "est", "pas", "vol", "el", "los", "pp", "u201d", "well-b", "http", "volumtype", "par", "0o", "0s", "3a", "3b", "3d", "6b", "6o", "a1", "a2", "a3", "a4", "ab", "ac", "ad", "ae", "af", "ag", "aj", "al", "an", "ao", "ap", "ar", "av", "aw", "ax", "ay", "az", "b1", "b2", "b3", "ba", "bc", "bd", "be", "bi", "bj", "bk", "bl", "bn", "bp", "br", "bs", "bt", "bu", "bx", "c1", "c2", "c3", "cc", "cd", "ce", "cf", "cg", "ch", "ci", "cj", "cl", "cm", "cn", "cp", "cq", "cr", "cs", "ct", "cu", "cv", "cx", "cy", "cz", "d2", "da", "dc", "dd", "de", "df", "di", "dj", "dk", "dl", "do", "dp", "dr", "ds", "dt", "du", "dx", "dy", "e2", "e3", "ea", "ec", "ed", "ee", "ef", "ei", "ej", "el", "em", "en", "eo", "ep", "eq", "er", "es", "et", "eu", "ev", "ex", "ey", "f2", "fa", "fc", "ff", "fi", "fj", "fl", "fn", "fo", "fr", "fs", "ft", "fu", "fy", "ga", "ge", "gi", "gj", "gl", "go", "gr", "gs", "gy", "h2", "h3", "hh", "hi", "hj", "ho", "hr", "hs", "hu", "hy", "i", "i2", "i3", "i4", "i6", "i7", "i8", "ia", "ib", "ic", "ie", "ig", "ih", "ii", "ij", "il", "in", "io", "ip", "iq", "ir", "iv", "ix", "iy", "iz", "jj", "jr", "js", "jt", "ju", "ke", "kg", "kj", "km", "ko", "l2", "la", "lb", "lc", "lf", "lj", "ln", "lo", "lr", "ls", "lt", "m2", "ml", "mn", "mo", "ms", "mt", "mu", "n2", "nc", "nd", "ne", "ng", "ni", "nj", "nl", "nn", "nr", "ns", "nt", "ny", "oa", "ob", "oc", "od", "of", "og", "oi", "oj", "ol", "om", "on", "oo", "oq", "or", "os", "ot", "ou", "ow", "ox", "oz", "p1", "p2", "p3", "pc", "pd", "pe", "pf", "ph", "pi", "pj", "pk", "pl", "pm", "pn", "po", "pq", "pr", "ps", "pt", "pu", "py", "qj", "qu", "r2", "ra", "rc", "rd", "rf", "rh", "ri", "rj", "rl", "rm", "rn", "ro", "rq", "rr", "rs", "rt", "ru", "rv", "ry", "s2", "sa", "sc", "sd", "se", "sf", "si", "sj", "sl", "sm", "sn", "sp", "sq", "sr", "ss", "st", "sy", "sz", "t1", "t2", "t3", "tb", "tc", "td", "te", "tf", "th", "ti", "tj", "tl", "tm", "tn", "tp", "tq", "tr", "ts", "tt", "tv", "tx", "ue", "ui", "uj", "uk", "um", "un", "uo", "ur", "ut", "va", "wa", "vd", "wi", "vj", "vo", "wo", "vq", "vt", "vu", "x1", "x2", "x3", "xf", "xi", "xj", "xk", "xl", "xn", "xo", "xs", "xt", "xv", "xx", "y2", "yj", "yl", "yr", "ys", "yt", "zi", "zz" };
        private HtmlWeb web;
        private HtmlDocument htmlDoc;

        public static WordListManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

		public IEnumerator GetResponseStatusCode(string keyword, Action<int> callback = null)
		{
			string url;
			string baseUrl = @"https://en.wikipedia.org/wiki/";

			url = baseUrl + keyword;
			web = new HtmlWeb();

			htmlDoc = web.Load(url);

			yield return null;
			callback((int)web.StatusCode);
		}

		public IEnumerator GetWordList(string keyword, Action<List<string>> callback = null)
		{
            List<string> result = new List<string>();
            Dictionary<string, int> termDictionary = new Dictionary<string, int>();
            HtmlDocument doc = GetWikiPage(keyword);

            var noArticleNode = doc.DocumentNode.SelectNodes("//div[contains(@class, 'noarticletext mw-content-ltr')]");

            if (noArticleNode != null)
            {
                //If page does not exist, check whether wiki has suggestion
                var suggestionNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'pagetitlecorrection')]");
                //If suggestion exists, get the link and open it
                if (suggestionNode != null)
                {
                    var suggestedLinkText = suggestionNode.SelectSingleNode("//td[contains(@class, 'mbox-text')]//a[@href]");
                    string newKeyword = suggestedLinkText.InnerText.Replace(' ', '_');
                    GetWordList(newKeyword);
                }
                else
                //Else, use the wiki search 
                {
                    var searchResult = GetWikiSearchPage(keyword);
                    //Choose the first search result presented
                    var resultUrls = GetLinksFromSearchPage(searchResult);
                    string relevantUrl = resultUrls[0];
                    doc = GetWikiPage(relevantUrl, true);
                    Console.WriteLine(keyword + "\t:\tPage does not exist, choosing top search result: " + relevantUrl);
                    result.Add("Wiki page related to keyword not found, using the top search result using keyword as query...");
                    termDictionary = GetTermsDictionary(doc);
                    //GetWikiList(doc);
                }
            }
            else
            {
                //Console.WriteLine(keyword + "\t:Page exists");
                result.Add("Word List Generated");
                termDictionary = GetTermsDictionary(doc);
				result.Add(keyword);
                //GetWikiList(doc);
            }

            foreach (var term in termDictionary.OrderByDescending(term => term.Value))
            {
                result.Add(term.Key);
            }

			yield return null;
			callback(result);
		}

        private Dictionary<string, int> GetTermsDictionary(HtmlDocument doc)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();

            //var paragraphs = doc.DocumentNode.SelectNodes("//div[contains(@id, 'mw-content-text')]");
            //Get the contents
            var paragraphs = doc.DocumentNode.SelectNodes("//p");

            var contents = doc.DocumentNode.SelectNodes("//div[contains(@class, 'mw-parser-output')]/p");
            var orderedLists = doc.DocumentNode.SelectNodes("//div[contains(@class, 'mw-parser-output')]/ol/li");
            var unorderedLists = doc.DocumentNode.SelectNodes("//div[contains(@class, 'mw-parser-output')]/ul/li");
            //int count = 1;
            var lemmatizer = new LemmatizerPrebuiltCompact(LemmaSharp.LanguagePrebuilt.English);

            if (contents != null)
            {
                foreach (var content in contents)
                {
                    //Console.WriteLine(content.InnerText);
                    string[] terms = content.InnerText.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var term in terms)
                    {
                        string input = Preprocess(term, lemmatizer);
                        //Don't count on stopwords
                        if (stopwords.Contains(input))
                            continue;
                        if (dict.ContainsKey(input))
                            dict[input] += 1;
                        else if (input.Length > 14)
                            continue;
                        else
                            if (!string.IsNullOrEmpty(input))
                            dict.Add(input, 1);
                    }
                }
            }

            if (orderedLists != null)
            {
                foreach (var content in orderedLists)
                {
                    //Console.WriteLine(content.InnerText);
                    string[] terms = content.InnerText.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var term in terms)
                    {
                        string input = Preprocess(term, lemmatizer);
                        //Don't count on stopwords
                        if (stopwords.Contains(input))
                            continue;
                        if (dict.ContainsKey(input))
                            dict[input] += 1;
                        else if (input.Length > 14)
                            continue;
                        else
                            if (!string.IsNullOrEmpty(input))
                            dict.Add(input, 1);
                    }
                }
            }

            if (unorderedLists != null)
            {
                foreach (var content in unorderedLists)
                {
                    //Console.WriteLine(content.InnerText);
                    string[] terms = content.InnerText.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var term in terms)
                    {
                        string input = Preprocess(term, lemmatizer);
                        //Don't count on stopwords
                        if (stopwords.Contains(input))
                            continue;
                        if (dict.ContainsKey(input))
                            dict[input] += 1;
                        else if (input.Length > 14)
                            continue;
                        else
                            if (!string.IsNullOrEmpty(input))
                            dict.Add(input, 1);
                    }
                }
            }

            return dict;
        }

        private string Preprocess(string term, LemmatizerPrebuiltCompact lemmatizer)
        {
            string result;

            char[] termCharArray = term.ToCharArray();
            //Remove non-alphanumeric letters
            termCharArray = Array.FindAll<char>(termCharArray, (ch => (char.IsLetterOrDigit(ch) || char.IsWhiteSpace(ch))));
            string input = new string(termCharArray.Where(char.IsLetter).ToArray());
            //string input = new string(termCharArray);
            //Remove newline character from a term
            input = input.Trim(new char[] { '\n' });
            //Make all words lowercase
            input = input.ToLower();
            //Lemmatize word
            result = lemmatizer.Lemmatize(input);

            return result;
        }

        private HtmlDocument RemoveTags(HtmlDocument doc, string tag)
        {
            string xpath = @"//" + tag;
            HtmlNodeCollection nodes = doc.DocumentNode.SelectNodes(xpath);

            if (nodes == null)
                return doc;

            foreach (HtmlNode node in nodes)
            {
                node.Remove();
            }

            return doc;
        }

		private HtmlDocument GetWikiPage(string keyword, bool isURL = false)
		{
            string url;

            if (!isURL)
            {
                string baseUrl = @"https://en.wikipedia.org/wiki/";

                url = baseUrl + keyword;
            }
            else
            {
                string baseUrl = @"https://en.wikipedia.org";
                url = baseUrl + keyword;
            }

			web = new HtmlWeb();

			htmlDoc = web.Load(url);

			htmlDoc = RemoveTags(htmlDoc, "script");
            htmlDoc = RemoveTags(htmlDoc, "link");

            return htmlDoc;
        }

        private HtmlDocument GetWikiSearchPage(string keyword)
        {
            //Get web page
            string qry = keyword.ToLower();

            string html = @"https://en.wikipedia.org/w/index.php?search=" + qry + "&title=Special%3ASearch&profile=advanced&fulltext=1&advancedSearch-current=%7B%7D&ns0=1";

            HtmlWeb web = new HtmlWeb();

            HtmlDocument htmlDoc = web.Load(html);

            // Preprocess
            htmlDoc = RemoveTags(htmlDoc, "script");
            htmlDoc = RemoveTags(htmlDoc, "link");

            return htmlDoc;
        }

        private List<string> GetLinksFromSearchPage(HtmlDocument doc, int limit = -1)
        {
            var resultList = doc.DocumentNode.SelectNodes("//div[contains(@class, 'mw-search-result-heading')]");
            List<string> links = new List<string>();
            int count = 0;
            foreach (var result in resultList)
            {
                string link = result.Descendants("a").ToList()[0].GetAttributeValue("href", null);
                links.Add(link);
                count++;
                if (limit != -1 && count > limit)
                    break;
            }

            return links;
        }
    }
}
