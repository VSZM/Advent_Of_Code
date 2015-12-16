using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Advent_Of_Code_11_20
{
	class Day12JasonBourne : ISolvable
	{
		public string Solve(string[] inputLines, bool isPart2)
		{
			string json_str = inputLines.Aggregate( (a, b) => a + b );

			dynamic json_obj = JsonConvert.DeserializeObject( json_str );

			int sum = 0;

			if ( isPart2 )
			{
				IEnumerable<JToken> descendants = json_obj.Descendants();

				for ( int i = 0 ; i < descendants.Count() ; ++i )
				{
					var jt = descendants.ElementAt( i );
					if ( jt is JObject && (( JObject )jt).PropertyValues().Contains( "red" ) )
					{
						jt.Replace( null );
					}
				}
			}

			foreach ( var jt in json_obj.Descendants() )
			{
				if ( jt is JValue && jt.Type == JTokenType.Integer )
				{
					sum += ( int )jt;
				}
			}

			return sum.ToString();
		}
	}
}
