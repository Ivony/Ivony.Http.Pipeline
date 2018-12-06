using System;
using System.Collections.Generic;
using System.Text;

namespace Ivony.Http.Pipeline.Routes
{
  internal enum SegmentType
  {
    Static,
    Dynamic,
    InfinityDynamic,
  }

  internal struct TemplateSegment
  {
    public TemplateSegment( string value, SegmentType type )
    {
      Value = value;
      Type = type;
    }

    public string Value { get; }
    public SegmentType Type { get; }


    internal static TemplateSegment CreateSegment( string value )
    {
      if ( value.StartsWith( '{' ) )
      {
        if ( value.EndsWith( "*}" ) )
          return new TemplateSegment( value.Substring( 1, value.Length - 3 ), SegmentType.InfinityDynamic );

        else
          return new TemplateSegment( value.Substring( 1, value.Length - 2 ), SegmentType.Dynamic );
      }
      else
        return new TemplateSegment( value, SegmentType.Static );
    }


  }

}
