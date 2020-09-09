Shader "Unlit/VideoShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" } 

 //CG代码必须用

//CGPROGRAM

//。。。

//ENDCG括起来

//pragma surface 是定义unity Surface Shader中的函数定义 
       CGPROGRAM 
       #pragma surface surf NoLighting alpha 

        //alpha的数值决定了一张贴图的透明度
   fixed4 LightingNoLighting(SurfaceOutput s, fixed3 lightDir, fixed atten) { 
            fixed4 c; 
            c.rgb = s.Albedo; 
            c.a = s.Alpha; 
            return c; 
       } 

       //声明了一个Input的结构体
       struct Input 
       { 
            float2 uv_MainTex; 
       }; 

       sampler2D _MainTex; 

      
       void surf(Input IN, inout SurfaceOutput o)
        { 

            o.Emission = tex2D(_MainTex, IN.uv_MainTex).rgb; 
            //将黑白的一部分的alpha值设为0     
            //这个阈值及大于小于号需要根据具体的电影文件来设置，一般在0.45-0.5之间
            if(IN.uv_MainTex.x >= 0.5)        
            { 
               o.Alpha=0;
            }
           //将另一半的画面的alpha值设置为黑白部分的RGB值
           else
           { 
              o.Alpha = tex2D(_MainTex, float2(IN.uv_MainTex.x+0.5, IN.uv_MainTex.y)).rgb; 
           }
           
       } 

       ENDCG 
  } 
}