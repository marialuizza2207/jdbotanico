#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class PavilhaoBuilder : EditorWindow
{
    [MenuItem("Tools/Build Pavilhão")]
    public static void BuildPavilhao()
    {
        var pavilhao = GameObject.Find("Pavilhao");
        if (pavilhao == null) { Debug.LogError("[PavilhaoBuilder] GameObject 'Pavilhao' nao encontrado!"); return; }

        Vector3 c = pavilhao.transform.position;
        float largura = 8f, profundidade = 6f, alturaColuna = 3f;
        float espessura = 0.3f;

        Shader shader = Shader.Find("Universal Render Pipeline/Lit")
                     ?? Shader.Find("Standard");

        Material CriarMat(Color cor)
        {
            var mat = new Material(shader);
            mat.color = cor;
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", cor);
            return mat;
        }

        var matColuna  = CriarMat(new Color(0.95f, 0.93f, 0.88f));
        var matTelhado = CriarMat(new Color(0.55f, 0.35f, 0.15f));

        var colunas = pavilhao.transform.Find("Colunas")?.gameObject
                   ?? new GameObject("Colunas") { transform = { parent = pavilhao.transform } };

        Vector3[] posicoesColuna = {
            c + new Vector3(-largura/2, alturaColuna/2, -profundidade/2),
            c + new Vector3( largura/2, alturaColuna/2, -profundidade/2),
            c + new Vector3(-largura/2, alturaColuna/2,  profundidade/2),
            c + new Vector3( largura/2, alturaColuna/2,  profundidade/2),
        };
        string[] nomesColuna = { "Coluna_FE", "Coluna_FD", "Coluna_TE", "Coluna_TD" };

        for (int i = 0; i < 4; i++)
        {
            var existente = colunas.transform.Find(nomesColuna[i]);
            if (existente != null) continue;

            var col = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            col.name = nomesColuna[i];
            col.transform.SetParent(colunas.transform);
            col.transform.position   = posicoesColuna[i];
            col.transform.localScale = new Vector3(espessura, alturaColuna / 2f, espessura);
            col.GetComponent<Renderer>().material = matColuna;
        }

        var telhado = pavilhao.transform.Find("Telhado")?.gameObject
                   ?? new GameObject("Telhado") { transform = { parent = pavilhao.transform } };

        var existenteTelhado = telhado.transform.Find("Telhado_Mesh");
        if (existenteTelhado == null)
        {
            var telhadoMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
            telhadoMesh.name = "Telhado_Mesh";
            telhadoMesh.transform.SetParent(telhado.transform);
            telhadoMesh.transform.position   = c + new Vector3(0, alturaColuna + 0.1f, 0);
            telhadoMesh.transform.localScale  = new Vector3(largura + espessura, 0.2f, profundidade + espessura);
            telhadoMesh.GetComponent<Renderer>().material = matTelhado;
        }

        // Banco
        var banco = GameObject.Find("Banco");
        if (banco != null)
        {
            banco.transform.position = c + new Vector3(0f, 0f, -profundidade / 2 + 1f);
            ConstruirBanco(banco, CriarMat(new Color(0.55f, 0.35f, 0.15f)));
        }

        // Poste de Luz
        var poste = GameObject.Find("Poste_Luz");
        if (poste != null) ConstruirPoste(poste, c + new Vector3(largura / 2 + 1.5f, 0, 0));

        // Árvores
        var matTronco = CriarMat(new Color(0.42f, 0.26f, 0.12f));
        var matCopa   = CriarMat(new Color(0.15f, 0.55f, 0.15f));

        Vector3[] posArvores = {
            c + new Vector3(-largura / 2 - 2f, 0, -profundidade / 2 - 2f),
            c + new Vector3( largura / 2 + 2f, 0, -profundidade / 2 - 2f),
            c + new Vector3( 0f, 0, profundidade / 2 + 3f),
        };

        for (int i = 1; i <= 3; i++)
        {
            var arvore = GameObject.Find($"Arvore_0{i}");
            if (arvore == null) continue;
            arvore.transform.position = posArvores[i - 1];
            ConstruirArvore(arvore, matTronco, matCopa);
        }

        EditorUtility.SetDirty(pavilhao);
        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("[PavilhaoBuilder] Concluido. Salve com Ctrl+S.");
    }

    static void ConstruirBanco(GameObject banco, Material mat)
    {
        var assento = banco.transform.Find("Assento")?.gameObject
                   ?? new GameObject("Assento") { transform = { parent = banco.transform } };

        if (assento.transform.Find("Assento_Mesh") == null)
        {
            var mesh = GameObject.CreatePrimitive(PrimitiveType.Cube);
            mesh.name = "Assento_Mesh";
            mesh.transform.SetParent(assento.transform);
            mesh.transform.localPosition = new Vector3(0, 0.5f, 0);
            mesh.transform.localScale    = new Vector3(2f, 0.1f, 0.5f);
            mesh.GetComponent<Renderer>().material = mat;
        }
    }

    static void ConstruirPoste(GameObject poste, Vector3 pos)
    {
        poste.transform.position = pos;
        var haste = poste.transform.Find("Haste")?.gameObject
                 ?? new GameObject("Haste") { transform = { parent = poste.transform } };

        if (haste.transform.Find("Haste_Mesh") == null)
        {
            var hasteMesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            hasteMesh.name = "Haste_Mesh";
            hasteMesh.transform.SetParent(haste.transform);
            hasteMesh.transform.localPosition = new Vector3(0, 1.5f, 0);
            hasteMesh.transform.localScale    = new Vector3(0.08f, 1.5f, 0.08f);
            hasteMesh.GetComponent<Renderer>().material = new Material(Shader.Find("Standard"));
        }

        var lampada = poste.transform.Find("Lampada")?.gameObject
                   ?? new GameObject("Lampada") { transform = { parent = poste.transform } };

        if (lampada.GetComponent<Light>() == null)
        {
            var luz = lampada.AddComponent<Light>();
            luz.type      = LightType.Point;
            luz.color     = new Color(1f, 0.95f, 0.75f);
            luz.intensity = 1.5f;
            luz.range     = 8f;
        }
        lampada.transform.localPosition = new Vector3(0, 3f, 0);
    }

    static void ConstruirArvore(GameObject arvore, Material matTronco, Material matCopa)
    {
        var tronco = arvore.transform.Find("Tronco")?.gameObject
                  ?? new GameObject("Tronco") { transform = { parent = arvore.transform } };

        if (tronco.transform.Find("Tronco_Mesh") == null)
        {
            var mesh = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            mesh.name = "Tronco_Mesh";
            mesh.transform.SetParent(tronco.transform);
            mesh.transform.localPosition = new Vector3(0, 1f, 0);
            mesh.transform.localScale    = new Vector3(0.3f, 1f, 0.3f);
            mesh.GetComponent<Renderer>().material = matTronco;
        }

        var copa = arvore.transform.Find("Copa")?.gameObject
                ?? new GameObject("Copa") { transform = { parent = arvore.transform } };

        if (copa.transform.Find("Copa_Mesh") == null)
        {
            var mesh = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            mesh.name = "Copa_Mesh";
            mesh.transform.SetParent(copa.transform);
            mesh.transform.localPosition = new Vector3(0, 2.5f, 0);
            mesh.transform.localScale    = new Vector3(1.8f, 1.8f, 1.8f);
            mesh.GetComponent<Renderer>().material = matCopa;
        }
    }
}
#endif
