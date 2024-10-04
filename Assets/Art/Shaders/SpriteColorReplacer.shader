// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Sprites/ColorReplacer"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[SingleLineTexture]_OriginalPalette("OriginalPalette", 2D) = "white" {}
		[SingleLineTexture]_NewPalette("NewPalette", 2D) = "white" {}
		[HideInInspector] _texcoord( "", 2D ) = "white" {}

	}

	SubShader
	{
		LOD 0

		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" "PreviewType"="Plane" "CanUseSpriteAtlas"="True" }

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha
		
		
		Pass
		{
		CGPROGRAM
			
			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			

			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform sampler1D _NewPalette;
			uniform float4 _MainTex_ST;
			uniform sampler1D _OriginalPalette;

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				
				
				IN.vertex.xyz +=  float3(0,0,0) ; 
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				fixed4 alpha = tex2D (_AlphaTex, uv);
				color.a = lerp (color.a, alpha.r, _EnableExternalAlpha);
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}
			
			fixed4 frag(v2f IN  ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				float4 tex1DNode172 = tex1D( _NewPalette, 0.9375 );
				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST.xy + _MainTex_ST.zw;
				float4 tex2DNode90 = tex2D( _MainTex, uv_MainTex );
				float4 tex1DNode168 = tex1D( _NewPalette, 0.8125 );
				float4 break72 = ( tex1DNode172 == tex2DNode90 ? tex1DNode172 : ( tex1DNode168 == tex2DNode90 ? tex1DNode168 : ( tex1D( _OriginalPalette, 0.6875 ) == tex2DNode90 ? tex1D( _NewPalette, 0.6875 ) : ( tex1D( _OriginalPalette, 0.5625 ) == tex2DNode90 ? tex1D( _NewPalette, 0.5625 ) : ( tex1D( _OriginalPalette, 0.4375 ) == tex2DNode90 ? tex1D( _NewPalette, 0.4375 ) : ( tex1D( _OriginalPalette, 0.3125 ) == tex2DNode90 ? tex1D( _NewPalette, 0.3125 ) : ( tex1D( _OriginalPalette, 0.1875 ) == tex2DNode90 ? tex1D( _NewPalette, 0.1875 ) : ( tex1D( _OriginalPalette, 0.0625 ) == tex2DNode90 ? tex1D( _NewPalette, 0.0625 ) : tex2DNode90 ) ) ) ) ) ) ) );
				float4 appendResult73 = (float4(break72.r , break72.g , break72.b , tex2DNode90.a));
				
				fixed4 c = appendResult73;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
	CustomEditor "ASEMaterialInspector"
	
	Fallback Off
}
/*ASEBEGIN
Version=19201
Node;AmplifyShaderEditor.SamplerNode;125;972.3207,899.0609;Inherit;True;Property;_TextureSample2;Texture Sample 1;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;124;694.423,901.7003;Inherit;True;Property;_TextureSample1;Texture Sample 1;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;127;1278.303,924.5887;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;150;745.947,819.5713;Inherit;False;Constant;_Float0;Float 0;2;0;Create;True;0;0;0;False;0;False;0.0625;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;25;1281.709,521.4114;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;149;1728.374,1221.041;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;151;1245.765,1106.771;Inherit;False;Constant;_Float1;Float 0;2;0;Create;True;0;0;0;False;0;False;0.1875;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.BreakToComponentsNode;72;2477.891,1068.821;Inherit;False;COLOR;1;0;COLOR;0,0,0,0;False;16;FLOAT;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT;5;FLOAT;6;FLOAT;7;FLOAT;8;FLOAT;9;FLOAT;10;FLOAT;11;FLOAT;12;FLOAT;13;FLOAT;14;FLOAT;15
Node;AmplifyShaderEditor.DynamicAppendNode;73;2649.865,1067.471;Inherit;False;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;42;2817.799,1068.189;Float;False;True;-1;2;ASEMaterialInspector;0;10;Sprites/ColorReplacer;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.SamplerNode;147;1153.543,1191.656;Inherit;True;Property;_TextureSample3;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;148;1428.655,1193.906;Inherit;True;Property;_TextureSample4;Texture Sample 1;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;152;1357.137,1493.336;Inherit;True;Property;_TextureSample5;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;153;1632.249,1495.586;Inherit;True;Property;_TextureSample6;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;154;1910.451,1500.166;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;155;1530.295,1410.36;Inherit;False;Constant;_Float2;Float 0;2;0;Create;True;0;0;0;False;0;False;0.3125;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;156;1642.274,1795.895;Inherit;True;Property;_TextureSample7;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;157;1917.386,1798.145;Inherit;True;Property;_TextureSample8;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;158;2195.588,1802.724;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;159;1815.432,1712.919;Inherit;False;Constant;_Float3;Float 0;2;0;Create;True;0;0;0;False;0;False;0.4375;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;160;1883.192,2110.562;Inherit;True;Property;_TextureSample9;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;161;2158.305,2112.812;Inherit;True;Property;_TextureSample10;Texture Sample 1;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;162;2436.508,2117.391;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;163;2056.35,2027.582;Inherit;False;Constant;_Float4;Float 0;2;0;Create;True;0;0;0;False;0;False;0.5625;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;165;2381.609,2431.931;Inherit;True;Property;_TextureSample12;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;166;2659.813,2436.51;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;164;2106.497,2428.681;Inherit;True;Property;_TextureSample11;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;167;2279.655,2346.701;Inherit;False;Constant;_Float5;Float 0;2;0;Create;True;0;0;0;False;0;False;0.6875;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;168;2598.133,2757.086;Inherit;True;Property;_TextureSample13;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Compare;169;2876.337,2761.665;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;170;2323.021,2753.836;Inherit;True;Property;_TextureSample14;Texture Sample 1;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;171;2496.179,2671.856;Inherit;False;Constant;_Float6;Float 0;2;0;Create;True;0;0;0;False;0;False;0.8125;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;172;2786.207,3069.438;Inherit;True;Property;_TextureSample15;Texture Sample 1;2;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;174;2511.095,3066.188;Inherit;True;Property;_TextureSample16;Texture Sample 1;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture1D;8;0;SAMPLER1D;;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TexturePropertyNode;123;482.449,1462.042;Inherit;True;Property;_NewPalette;NewPalette;1;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;None;8d7f4209119acd44a83518b101d07ec7;False;white;LockedToTexture1D;Texture1D;-1;0;2;SAMPLER1D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.RangedFloatNode;175;2684.253,2984.208;Inherit;False;Constant;_Float7;Float 0;2;0;Create;True;0;0;0;False;0;False;0.9375;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;122;666.2307,1260.224;Inherit;True;Property;_OriginalPalette;OriginalPalette;0;1;[SingleLineTexture];Create;True;0;0;0;False;0;False;None;9c7ef049014d63d4d964dc832d17b370;False;white;LockedToTexture1D;Texture1D;-1;0;2;SAMPLER1D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.Compare;173;3064.411,3074.017;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;90;1473.14,520.8104;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.CustomExpressionNode;136;545.3821,628.7602;Float;False;float increment = 1 / Width@$float start = increment / 2.0f@$for (float f = start@ f < 0.99f@ f += increment)${$	return f@$}$return 1@$;4;Create;2;True;Width;FLOAT;0;In;;Float;False;True;PixelColor;FLOAT4;0,0,0,0;InOut;;Float;False;Loop Over Pixels 1D;True;False;0;;False;2;0;FLOAT;0;False;1;FLOAT4;0,0,0,0;False;2;FLOAT4;0;FLOAT4;2
Node;AmplifyShaderEditor.DynamicAppendNode;176;549.5461,788.3541;Inherit;False;FLOAT4;4;0;FLOAT4;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
WireConnection;125;0;123;0
WireConnection;125;1;150;0
WireConnection;124;0;122;0
WireConnection;124;1;150;0
WireConnection;127;0;124;0
WireConnection;127;1;90;0
WireConnection;127;2;125;0
WireConnection;127;3;90;0
WireConnection;149;0;147;0
WireConnection;149;1;90;0
WireConnection;149;2;148;0
WireConnection;149;3;127;0
WireConnection;72;0;173;0
WireConnection;73;0;72;0
WireConnection;73;1;72;1
WireConnection;73;2;72;2
WireConnection;73;3;90;4
WireConnection;42;0;73;0
WireConnection;147;0;122;0
WireConnection;147;1;151;0
WireConnection;148;0;123;0
WireConnection;148;1;151;0
WireConnection;152;0;122;0
WireConnection;152;1;155;0
WireConnection;153;0;123;0
WireConnection;153;1;155;0
WireConnection;154;0;152;0
WireConnection;154;1;90;0
WireConnection;154;2;153;0
WireConnection;154;3;149;0
WireConnection;156;0;122;0
WireConnection;156;1;159;0
WireConnection;157;0;123;0
WireConnection;157;1;159;0
WireConnection;158;0;156;0
WireConnection;158;1;90;0
WireConnection;158;2;157;0
WireConnection;158;3;154;0
WireConnection;160;0;122;0
WireConnection;160;1;163;0
WireConnection;161;0;123;0
WireConnection;161;1;163;0
WireConnection;162;0;160;0
WireConnection;162;1;90;0
WireConnection;162;2;161;0
WireConnection;162;3;158;0
WireConnection;165;0;123;0
WireConnection;165;1;167;0
WireConnection;166;0;164;0
WireConnection;166;1;90;0
WireConnection;166;2;165;0
WireConnection;166;3;162;0
WireConnection;164;0;122;0
WireConnection;164;1;167;0
WireConnection;168;0;123;0
WireConnection;168;1;171;0
WireConnection;169;0;168;0
WireConnection;169;1;90;0
WireConnection;169;2;168;0
WireConnection;169;3;166;0
WireConnection;170;0;122;0
WireConnection;170;1;171;0
WireConnection;172;0;123;0
WireConnection;172;1;175;0
WireConnection;174;0;122;0
WireConnection;174;1;175;0
WireConnection;173;0;172;0
WireConnection;173;1;90;0
WireConnection;173;2;172;0
WireConnection;173;3;169;0
WireConnection;90;0;25;0
WireConnection;176;0;136;2
ASEEND*/
//CHKSM=CE7C2599AA944071D37CCC5CC88F9D57C8CEA53D