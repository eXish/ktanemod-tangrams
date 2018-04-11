Shader "Custom/DisplayBar" {
Properties {
	_MainTex ("Base (RGB)", 2D) = "white" {}
	_OverlayTex ("Overlay (A)", 2D) = "white" {}
	_Progress ("Progress", Range(0.0, 1.0)) = 0.0
}

SubShader {
	Tags { "RenderType"="TransparentCutout" }
	LOD 150
	
	Pass {  
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : SV_POSITION;
				half2 texcoord : TEXCOORD0;
				half2 overlayTexcoord : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			sampler2D _OverlayTex;
			float4 _OverlayTex_ST;

			float _Progress;

			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.overlayTexcoord = TRANSFORM_TEX(v.texcoord, _OverlayTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.texcoord) * tex2D(_OverlayTex, i.overlayTexcoord).a;
				col.a = i.texcoord.x;
				clip((_Progress - 0.01) - col.a);

				return col;
			}
		ENDCG
	}
}

}
