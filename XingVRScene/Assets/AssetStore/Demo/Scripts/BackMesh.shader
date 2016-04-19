﻿// Copyright 2016 Baofeng Mojing Inc. All rights reserved.

Shader "Demo/BackMesh" {
  Properties {
    _MainTex ("Texture", 2D) = "white" {}
  }
  Category {
   Tags { "Queue"="Background" }
    ZWrite Off
    Lighting Off
    Fog {Mode Off}
    SubShader {
      Pass {
	    //Cull front
        SetTexture [_MainTex] {
          Combine texture
        }
      }
    }
  }
}
