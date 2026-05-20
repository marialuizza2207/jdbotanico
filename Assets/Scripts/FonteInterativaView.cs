using System.Collections.Generic;
using UnityEngine;

public class FonteInterativaView : MonoBehaviour
{
    private List<Renderer> renderers = new List<Renderer>();
    private Material matPedra;
    private ParticleSystem pingos;

    static readonly Color corPedra = new Color(0.55f, 0.50f, 0.45f);
    static readonly Color corHover = new Color(0.72f, 0.67f, 0.62f);

    void Awake()
    {
        var rootRend = GetComponent<Renderer>();
        if (rootRend != null) rootRend.enabled = false;

        matPedra = new Material(Shader.Find("Standard")) { color = corPedra };

        // Estrutura da fonte: base larga → coluna fina → bacia
        //   Cilindro Unity = 2 unidades de altura no local; scale.y multiplica essa altura.
        //   bottom = pos.y - scale.y  |  top = pos.y + scale.y
        renderers.Add(CriarCilindro("Base",   new Vector3(0, 0.20f, 0), new Vector3(1.4f, 0.20f, 1.4f)));
        renderers.Add(CriarCilindro("Coluna", new Vector3(0, 0.80f, 0), new Vector3(0.28f, 0.60f, 0.28f)));
        renderers.Add(CriarCilindro("Bacia",  new Vector3(0, 1.42f, 0), new Vector3(1.1f, 0.13f, 1.1f)));

        CriarPingos(spawnY: 1.56f);
    }

    Renderer CriarCilindro(string nome, Vector3 pos, Vector3 escala)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        go.name = nome;
        go.transform.SetParent(transform, false);
        go.transform.localPosition = pos;
        go.transform.localScale    = escala;
        var rend = go.GetComponent<Renderer>();
        rend.material = matPedra;
        Destroy(go.GetComponent<Collider>());
        return rend;
    }

    void CriarPingos(float spawnY)
    {
        var go = new GameObject("Pingos");
        go.transform.SetParent(transform, false);
        go.transform.localPosition = new Vector3(0, spawnY, 0);

        pingos = go.AddComponent<ParticleSystem>();

        var psr = go.GetComponent<ParticleSystemRenderer>();
        var matAgua = new Material(Shader.Find("Particles/Standard Unlit"));
        matAgua.color = new Color(0.3f, 0.75f, 1.0f, 0.88f);
        psr.material = matAgua;

        var main = pingos.main;
        main.loop            = true;
        main.startLifetime   = new ParticleSystem.MinMaxCurve(0.9f, 1.5f);
        main.startSpeed      = new ParticleSystem.MinMaxCurve(2.0f, 3.5f);
        main.startSize       = new ParticleSystem.MinMaxCurve(0.04f, 0.10f);
        main.startColor      = new Color(0.45f, 0.78f, 1.0f, 0.88f);
        main.gravityModifier = 2.2f;
        main.simulationSpace = ParticleSystemSimulationSpace.World;
        main.playOnAwake     = false;

        var emission = pingos.emission;
        emission.rateOverTime = 70f;

        var shape = pingos.shape;
        shape.shapeType = ParticleSystemShapeType.Cone;
        shape.angle     = 22f;
        shape.radius    = 0.12f;

        var col = pingos.colorOverLifetime;
        col.enabled = true;
        var grad = new Gradient();
        grad.SetKeys(
            new[] { new GradientColorKey(new Color(0.7f, 0.95f, 1f), 0f),
                    new GradientColorKey(new Color(0.2f, 0.60f, 1f), 1f) },
            new[] { new GradientAlphaKey(0.9f, 0f),
                    new GradientAlphaKey(0.0f, 1f) }
        );
        col.color = grad;

        pingos.Stop();
    }

    public void SetHover(bool hover)
    {
        if (matPedra != null)
            matPedra.color = hover ? corHover : corPedra;
    }

    public void SetAtivo(bool ativo)
    {
        if (pingos == null) return;
        if (ativo) pingos.Play();
        else       pingos.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }
}
