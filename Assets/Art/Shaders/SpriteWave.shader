// Upgrade NOTE: upgraded instancing buffer 'SpriteWave' to new syntax.

// Made with Amplify Shader Editor v1.9.2.1
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "SpriteWave"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		_V_Speed("V_Speed", Range( -10 , 10)) = 1
		_V_Amplitude("V_Amplitude", Range( 1 , 2)) = 1
		_V_Frequency("V_Frequency", Range( 0.01 , 2)) = 1
		_H_Speed("H_Speed", Range( -10 , 10)) = 1
		_H_Amplitude("H_Amplitude", Range( 1 , 2)) = 1
		_H_Frequency("H_Frequency", Range( 0.01 , 2)) = 1
		_TimeOffset("TimeOffset", Float) = 0

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
			#include "UnityShaderVariables.cginc"
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
				
			};
			
			uniform fixed4 _Color;
			uniform float _EnableExternalAlpha;
			uniform sampler2D _MainTex;
			uniform sampler2D _AlphaTex;
			uniform float _H_Amplitude;
			float4 _MainTex_TexelSize;
			uniform float _H_Frequency;
			uniform float _H_Speed;
			uniform float _V_Amplitude;
			uniform float _V_Frequency;
			uniform float _V_Speed;
			UNITY_INSTANCING_BUFFER_START(SpriteWave)
				UNITY_DEFINE_INSTANCED_PROP(float4, _MainTex_ST)
#define _MainTex_ST_arr SpriteWave
				UNITY_DEFINE_INSTANCED_PROP(float, _TimeOffset)
#define _TimeOffset_arr SpriteWave
			UNITY_INSTANCING_BUFFER_END(SpriteWave)

			
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

				float4 _MainTex_ST_Instance = UNITY_ACCESS_INSTANCED_PROP(_MainTex_ST_arr, _MainTex_ST);
				float2 uv_MainTex = IN.texcoord.xy * _MainTex_ST_Instance.xy + _MainTex_ST_Instance.zw;
				float _TimeOffset_Instance = UNITY_ACCESS_INSTANCED_PROP(_TimeOffset_arr, _TimeOffset);
				float mulTime28 = _Time.y * _H_Speed;
				float mulTime86 = _Time.y * _V_Speed;
				float2 appendResult20 = (float2(( ( floor( ( _H_Amplitude * sin( ( ( floor( ( _MainTex_TexelSize.z * uv_MainTex.y ) ) * _H_Frequency ) + ( _TimeOffset_Instance + mulTime28 ) ) ) ) ) * 0.05 ) + uv_MainTex.x ) , ( uv_MainTex.y + ( floor( ( _V_Amplitude * sin( ( ( floor( ( uv_MainTex.x * _MainTex_TexelSize.w ) ) * _V_Frequency ) + ( mulTime86 + _TimeOffset_Instance ) ) ) ) ) * 0.05 ) )));
				
				fixed4 c = tex2D( _MainTex, appendResult20 );
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
Node;AmplifyShaderEditor.CommentaryNode;104;-648.1913,-20.15178;Inherit;False;1160.402;567.8846;Vertical;14;102;93;87;88;89;90;97;91;95;96;98;94;92;86;;1,1,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;103;-610.9261,-1228.035;Inherit;False;1091.751;554.4146;Horizontal;14;85;79;28;101;23;74;78;82;21;27;83;84;76;22;;1,1,1,1;0;0
Node;AmplifyShaderEditor.SimpleTimeNode;86;-262.0677,358.3968;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;92;-407.4092,164.5258;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;94;-189.7885,39.85564;Inherit;False;Property;_V_Amplitude;V_Amplitude;1;0;Create;True;0;0;0;False;0;False;1;1;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;96;-544.4579,447.6135;Inherit;False;Property;_V_Speed;V_Speed;0;0;Create;True;0;0;0;False;0;False;1;3;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;95;-538.8101,283.7633;Inherit;False;Property;_V_Frequency;V_Frequency;2;0;Create;True;0;0;0;False;0;False;1;0.57;0.01;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;91;-564.8901,168.9008;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;88;-44.50418,164.9937;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;87;-149.1923,167.8878;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;93;-292.5801,169.1628;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.33;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;102;-42.46912,369.9944;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;22;-584.137,-1030.09;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;10;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;76;-426.6562,-1034.465;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;84;-313.8265,-1036.828;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0.33;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;83;-209.0352,-1159.135;Inherit;False;Property;_H_Amplitude;H_Amplitude;4;0;Create;True;0;0;0;False;0;False;1;1;1;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;27;-168.439,-1031.103;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SinOpNode;21;-63.75083,-1033.997;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;82;39.3667,-1059.107;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;78;183.1175,-1058.718;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;74;299.1417,-1055.833;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;23;85.58323,-938.0272;Inherit;False;Constant;_PixelOffset;PixelOffset;1;0;Create;True;0;0;0;False;0;False;0.05;10;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;85;-497.166,-916.627;Inherit;False;Property;_H_Frequency;H_Frequency;5;0;Create;True;0;0;0;False;0;False;1;0.64;0.01;2;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;79;-547.1417,-782.4468;Inherit;False;Property;_H_Speed;H_Speed;3;0;Create;True;0;0;0;False;0;False;1;-3;-10;10;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleTimeNode;28;-259.7661,-835.0593;Inherit;False;1;0;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;101;-74.87628,-823.0432;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;26;-98.98924,-392.5865;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;99;-98.36913,-215.7312;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;100;-1318.33,-379.5948;Inherit;False;InstancedProperty;_TimeOffset;TimeOffset;6;0;Create;True;0;0;0;False;0;False;0;1.27;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.DynamicAppendNode;20;156.6828,-339.8152;Inherit;True;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;4;407.4507,-339.5479;Inherit;True;Property;_TextureSample0;Texture Sample 0;1;0;Create;True;0;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;0;721.9585,-341.4315;Float;False;True;-1;2;ASEMaterialInspector;0;10;SpriteWave;0f8ba0101102bb14ebf021ddadce9b49;True;SubShader 0 Pass 0;0;0;SubShader 0 Pass 0;2;False;True;3;1;False;;10;False;;0;1;False;;0;False;;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;False;False;False;False;False;False;False;False;False;True;2;False;;False;False;True;5;Queue=Transparent=Queue=0;IgnoreProjector=True;RenderType=Transparent=RenderType;PreviewType=Plane;CanUseSpriteAtlas=True;False;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;0;;0;0;Standard;0;0;1;True;False;;False;0
Node;AmplifyShaderEditor.TexelSizeNode;77;-954.9832,-493.0455;Inherit;False;-1;1;0;SAMPLER2D;;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;13;-746.5578,-313.3993;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TemplateShaderPropertyNode;105;-1046.173,-302.6994;Inherit;False;0;0;_MainTex;Shader;False;0;5;SAMPLER2D;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;89;58.61316,139.8837;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;2;False;1;FLOAT;0
Node;AmplifyShaderEditor.FloorOpNode;90;202.3647,140.2727;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;97;321.7003,141.5019;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;98;104.8298,260.9633;Inherit;False;Constant;_PixelOffset1;PixelOffset;1;0;Create;True;0;0;0;False;0;False;0.05;10;0;0;0;1;FLOAT;0
WireConnection;86;0;96;0
WireConnection;92;0;91;0
WireConnection;91;0;13;1
WireConnection;91;1;77;4
WireConnection;88;0;87;0
WireConnection;87;0;93;0
WireConnection;87;1;102;0
WireConnection;93;0;92;0
WireConnection;93;1;95;0
WireConnection;102;0;86;0
WireConnection;102;1;100;0
WireConnection;22;0;77;3
WireConnection;22;1;13;2
WireConnection;76;0;22;0
WireConnection;84;0;76;0
WireConnection;84;1;85;0
WireConnection;27;0;84;0
WireConnection;27;1;101;0
WireConnection;21;0;27;0
WireConnection;82;0;83;0
WireConnection;82;1;21;0
WireConnection;78;0;82;0
WireConnection;74;0;78;0
WireConnection;74;1;23;0
WireConnection;28;0;79;0
WireConnection;101;0;100;0
WireConnection;101;1;28;0
WireConnection;26;0;74;0
WireConnection;26;1;13;1
WireConnection;99;0;13;2
WireConnection;99;1;97;0
WireConnection;20;0;26;0
WireConnection;20;1;99;0
WireConnection;4;0;105;0
WireConnection;4;1;20;0
WireConnection;0;0;4;0
WireConnection;77;0;105;0
WireConnection;13;2;105;0
WireConnection;89;0;94;0
WireConnection;89;1;88;0
WireConnection;90;0;89;0
WireConnection;97;0;90;0
WireConnection;97;1;98;0
ASEEND*/
//CHKSM=ECF735EC2A5FA9D60562003D7A595D835B5D62C3