﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDTO.Basic.JsonNewtonSoft
{
	public partial class MyDTO
	{
		public string? Field05
		{
			get
			{
				short length = this.Field05_Length;
				return length switch
				{
					< 0 => null,
					0 => string.Empty,
					_ => Encoding.UTF8.GetString(Field05_Data.AsSpan().Slice(0, length))
				};
			}
			set
			{
				if (value is null)
				{
					this.Field05_Length = -1;
				}
				else if (value.Length == 0)
				{
					this.Field05_Length = 0;
				}
				else
				{
					var buffer = Encoding.UTF8.GetBytes(value);
					this.Field05_Data = buffer;
					this.Field05_Length = (short)buffer.Length;
				}
			}
		}
	}
}