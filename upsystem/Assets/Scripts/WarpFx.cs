using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class WarpFx : MonoBehaviour
{
    private Shader _warpShader;
    private Material _warpMat;
    public float displaceAmount = 10;
    public Texture2D displaceTex;


    IEnumerator WarpFxRoutine()
    {
        displaceAmount = 0.01f;
        for (int i = 0; i < 50; i++)
        {
            displaceAmount *= 1.02f;
            yield return new WaitForSeconds(0.01f);
        }
        for (int i = 0; i < 14; i++)
        {
            displaceAmount *= 2f;
            yield return new WaitForSeconds(0.01f);
        }
        GameStateManager.Instance.Jump();
        displaceAmount = 0;
    }

    public void JumpHandler()
    {
        StartCoroutine("WarpFxRoutine");
    }
    void Start()
    {
        _warpShader = Shader.Find("Custom/distort");
        _warpMat = new Material(_warpShader);
        _warpMat.SetTexture("_DisplaceTex", displaceTex);

    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination, _warpMat);
    }

    void Update()
    {
        _warpMat.SetFloat("_Amount", displaceAmount);
    }
}