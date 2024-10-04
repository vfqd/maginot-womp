// Upgrade NOTE: upgraded instancing buffer 'GroundPlane' to new syntax.

// Made with Amplify Shader Editor v1.9.6.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "GroundPlane"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_ReferenceTexture("ReferenceTexture", 2D) = "white" {}
		_PixelsPerUnit("Pixels Per Unit", Float) = 0
		_Offset("Offset", Vector) = (0,0,0,0)
		_CellDiscolor("CellDiscolor", Color) = (0.9411765,0.8039709,0.5843138,1)

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
			#pragma multi_compile_instancing


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
				float4 ase_texcoord1 : TEXCOORD1;
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float4 _CellDiscolor;
			uniform sampler2D _ReferenceTexture;
			float4 _ReferenceTexture_TexelSize;
			uniform float _PixelsPerUnit;
			UNITY_INSTANCING_BUFFER_START(GroundPlane)
				UNITY_DEFINE_INSTANCED_PROP(float2, _Offset)
#define _Offset_arr GroundPlane
			UNITY_INSTANCING_BUFFER_END(GroundPlane)

			
			v2f vert( appdata_t IN  )
			{
				v2f OUT;
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
				UNITY_TRANSFER_INSTANCE_ID(IN, OUT);
				float3 ase_worldPos = mul(unity_ObjectToWorld, float4( (IN.vertex).xyz, 1 )).xyz;
				OUT.ase_texcoord1.xyz = ase_worldPos;
				
				
				//setting value to unused interpolator channels and avoid initialization warnings
				OUT.ase_texcoord1.w = 0;
				
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

				float3 ase_worldPos = IN.ase_texcoord1.xyz;
				float2 appendResult12 = (float2(( ase_worldPos.x / _ReferenceTexture_TexelSize.z ) , ( ase_worldPos.y / _ReferenceTexture_TexelSize.w )));
				float2 _Offset_Instance = UNITY_ACCESS_INSTANCED_PROP(_Offset_arr, _Offset);
				float4 tex2DNode2 = tex2D( _ReferenceTexture, ( ( appendResult12 * _PixelsPerUnit ) + _Offset_Instance ) );
				float4 appendResult21 = (float4(tex2DNode2.r , tex2DNode2.g , tex2DNode2.b , 1.0));
				
				fixed4 c = ( ( ( ( round( ase_worldPos.x ) + round( ase_worldPos.y ) ) % 2.0 ) == 0.0 ? ( _CellDiscolor * appendResult21 ) : appendResult21 ) * _Color );
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
Version=19603
Node;AmplifyShaderEditor.TexturePropertyNode;1;-1101.726,-281.35;Inherit;True;Property;_ReferenceTexture;ReferenceTexture;0;0;Create;True;0;0;0;False;0;False;None;d8142a79871179644aca3f7a8f8eb1ab;False;white;Auto;Texture2D;-1;0;2;SAMPLER2D;0;SAMPLERSTATE;1
Node;AmplifyShaderEditor.WorldPosInputsNode;3;-850.2064,71.65822;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexelSizeNode;9;-911.683,221.9355;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleDivideOpNode;6;-566.4122,20.35761;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;10;-565.683,119.9355;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;12;-427.683,43.93555;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RangedFloatNode;5;-595.8109,328.5578;Inherit;False;Property;_PixelsPerUnit;Pixels Per Unit;1;0;Create;True;0;0;0;False;0;False;0;16;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-272.683,40.93555;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;24;-259.7935,175.2269;Inherit;False;InstancedProperty;_Offset;Offset;3;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SimpleAddOpNode;25;-108.6138,88.08118;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.WorldPosInputsNode;26;370.5497,-804.4124;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.SamplerNode;2;-97.83987,-216.4395;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RoundOpNode;39;562.3471,-735.1215;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RoundOpNode;38;563.3471,-824.1215;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;21;300.8141,-223.7498;Inherit;True;COLOR;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;1;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleAddOpNode;28;710.7255,-767.9057;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;33;540.8507,-582.5388;Inherit;False;Property;_CellDiscolor;CellDiscolor;4;0;Create;True;0;0;0;False;0;False;0.9411765,0.8039709,0.5843138,1;1,0.9256168,0.8066038,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;31;890.7468,-553.7994;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleRemainderNode;29;890.2382,-741.079;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.Compare;37;1121.347,-497.1215;Inherit;False;0;4;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;22;1905.62,-367.9887;Inherit;True;0;0;_Color;Shader;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.WorldPosInputsNode;44;819.9376,-1054.199;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.DynamicAppendNode;43;1021.134,-972.4;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;40;1011.427,-1103.789;Inherit;False;Property;_Center;Center;5;0;Create;True;0;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;47;1010.897,-1268.844;Inherit;False;Constant;_Origin;Origin;5;0;Create;True;0;0;0;False;0;False;0,0;30,18.5;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.DistanceOpNode;41;1248.426,-1015.789;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.DistanceOpNode;46;1247.896,-1202.843;Inherit;False;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleDivideOpNode;48;1425.896,-1107.843;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;49;1221.152,-914.8195;Inherit;False;Constant;_White;White;6;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ColorNode;50;1229.207,-738.8318;Inherit;False;Property;_PoisonTint;PoisonTint;6;0;Create;True;0;0;0;False;0;False;0.8911108,0.740566,1,1;1,1,1,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.LerpOp;45;1520.357,-862.9496;Inherit;False;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;23;1488,-592;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SamplerNode;18;-310.3322,-537.3842;Inherit;True;Property;_TextureSample1;Texture Sample 1;3;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.ColorNode;15;-91.8025,-734.9202;Inherit;False;Property;_MaskColor;MaskColor;2;0;Create;True;0;0;0;False;0;False;0,0,0,0;0.5411765,0.6941177,0.254902,1;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.Compare;16;180.398,-479.4208;Inherit;False;0;4;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;3;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;17;-596.0026,-529.0214;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;51;1975.935,-504.3156;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;2373.595,-515.9722;Float;False;True;-1;2;ASEMaterialInspector;0;10;GroundPlane;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
WireConnection;9;0;1;0
WireConnection;6;0;3;1
WireConnection;6;1;9;3
WireConnection;10;0;3;2
WireConnection;10;1;9;4
WireConnection;12;0;6;0
WireConnection;12;1;10;0
WireConnection;13;0;12;0
WireConnection;13;1;5;0
WireConnection;25;0;13;0
WireConnection;25;1;24;0
WireConnection;2;0;1;0
WireConnection;2;1;25;0
WireConnection;39;0;26;2
WireConnection;38;0;26;1
WireConnection;21;0;2;1
WireConnection;21;1;2;2
WireConnection;21;2;2;3
WireConnection;28;0;38;0
WireConnection;28;1;39;0
WireConnection;31;0;33;0
WireConnection;31;1;21;0
WireConnection;29;0;28;0
WireConnection;37;0;29;0
WireConnection;37;2;31;0
WireConnection;37;3;21;0
WireConnection;43;0;44;1
WireConnection;43;1;44;2
WireConnection;41;0;40;0
WireConnection;41;1;43;0
WireConnection;46;0;47;0
WireConnection;46;1;40;0
WireConnection;48;0;41;0
WireConnection;48;1;46;0
WireConnection;45;0;49;0
WireConnection;45;1;50;0
WireConnection;45;2;48;0
WireConnection;23;0;45;0
WireConnection;18;0;17;0
WireConnection;16;0;15;0
WireConnection;16;1;18;0
WireConnection;16;2;2;0
WireConnection;16;3;18;0
WireConnection;51;0;37;0
WireConnection;51;1;22;0
WireConnection;0;0;51;0
ASEEND*/
//CHKSM=B97B08CC978CB29DECFB46DA88141FAE54145E68