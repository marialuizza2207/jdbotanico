#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

public class FixJardim : EditorWindow
{
    [MenuItem("Tools/Fix Jardim")]
    public static void Fix()
    {
        Shader shader = null;
        var rp = GraphicsSettings.defaultRenderPipeline;
        if (rp != null)
        {
            shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Simple Lit");
            if (shader == null) shader = Shader.Find("Universal Render Pipeline/Unlit");
        }
        if (shader == null) shader = Shader.Find("Standard");
        if (shader == null) { Debug.LogError("❌ Nenhum shader encontrado!"); return; }
        Debug.Log("✅ Shader: " + shader.name);

        System.IO.Directory.CreateDirectory("Assets/Materials");

        Material CriarMat(string nome, Color cor)
        {
            string path = $"Assets/Materials/{nome}.mat";
            var mat = new Material(shader) { name = nome };
            mat.color = cor;
            if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", cor);
            if (mat.HasProperty("_Color"))     mat.SetColor("_Color",     cor);
            AssetDatabase.CreateAsset(mat, path);
            return mat;
        }

        foreach (var guid in AssetDatabase.FindAssets("t:Material", new[] { "Assets/Materials" }))
            AssetDatabase.DeleteAsset(AssetDatabase.GUIDToAssetPath(guid));

        var matGramado  = CriarMat("Mat_Gramado",  new Color(0.20f, 0.55f, 0.20f));
        var matColuna   = CriarMat("Mat_Coluna",   new Color(0.95f, 0.93f, 0.88f));
        var matTelhado  = CriarMat("Mat_Telhado",  new Color(0.55f, 0.35f, 0.15f));
        var matBanco    = CriarMat("Mat_Banco",    new Color(0.45f, 0.25f, 0.10f));
        var matTronco   = CriarMat("Mat_Tronco",   new Color(0.42f, 0.26f, 0.12f));
        var matCopa     = CriarMat("Mat_Copa",     new Color(0.15f, 0.55f, 0.15f));
        var matRosa     = CriarMat("Mat_Rosa",     new Color(1.00f, 0.40f, 0.60f));
        var matTulipa   = CriarMat("Mat_Tulipa",   new Color(1.00f, 0.65f, 0.00f));
        var matOrquidea = CriarMat("Mat_Orquidea", new Color(0.70f, 0.00f, 0.80f));
        var matGirassol = CriarMat("Mat_Girassol", new Color(1.00f, 0.85f, 0.00f));
        var matLavanda  = CriarMat("Mat_Lavanda",  new Color(0.65f, 0.50f, 0.90f));
        var matFonte    = CriarMat("Mat_Fonte",    new Color(0.20f, 0.60f, 1.00f));

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        void AplicarMat(Renderer r, Material m)
        {
            if (r && m) { r.sharedMaterial = m; EditorUtility.SetDirty(r.gameObject); }
        }

        // ── Gramado ──────────────────────────────────────────────
        var gramado = GameObject.Find("Plano_Gramado");
        if (gramado != null)
        {
            gramado.transform.position   = Vector3.zero;
            gramado.transform.localScale = new Vector3(3f, 1f, 3f);
            AplicarMat(gramado.GetComponent<Renderer>(), matGramado);
            EditorUtility.SetDirty(gramado);
            Debug.Log("✅ Plano_Gramado corrigido");
        }

        // ── Pavilhão ─────────────────────────────────────────────
        var pavilhao = GameObject.Find("Pavilhao");
        if (pavilhao != null)
        {
            pavilhao.transform.position = new Vector3(0f, 0f, 8f);
            foreach (var rend in pavilhao.GetComponentsInChildren<Renderer>())
            {
                string n = rend.gameObject.name;
                if      (n.Contains("Coluna"))  AplicarMat(rend, matColuna);
                else if (n.Contains("Telhado")) AplicarMat(rend, matTelhado);
            }
            EditorUtility.SetDirty(pavilhao);
            Debug.Log("✅ Pavilhão corrigido");
        }

        // ── Banco ────────────────────────────────────────────────
        var banco = GameObject.Find("Banco");
        if (banco != null)
        {
            foreach (var rend in banco.GetComponentsInChildren<Renderer>())
                AplicarMat(rend, matBanco);
            EditorUtility.SetDirty(banco);
            Debug.Log("✅ Banco corrigido");
        }

        // ── Árvores ──────────────────────────────────────────────
        for (int i = 1; i <= 3; i++)
        {
            var arvore = GameObject.Find($"Arvore_0{i}");
            if (arvore == null) continue;
            foreach (var rend in arvore.GetComponentsInChildren<Renderer>())
            {
                string n = rend.gameObject.name;
                if      (n.Contains("Tronco")) AplicarMat(rend, matTronco);
                else if (n.Contains("Copa"))   AplicarMat(rend, matCopa);
            }
            EditorUtility.SetDirty(arvore);
            Debug.Log($"✅ Arvore_0{i} corrigida");
        }

        // ── Flores Coletáveis ────────────────────────────────────
        Material[] matsFlores = { matRosa, matTulipa, matOrquidea, matGirassol, matLavanda };
        Vector3[] posFlores = {
            new Vector3(-3f, 0.5f,  2f),
            new Vector3( 3f, 0.5f,  2f),
            new Vector3(-3f, 0.5f, -2f),
            new Vector3( 3f, 0.5f, -2f),
            new Vector3( 0f, 0.5f,  4f),
        };
        for (int i = 1; i <= 5; i++)
        {
            var go = GameObject.Find($"Flor_Coletavel_0{i}");
            if (go == null) continue;
            go.transform.position   = posFlores[i - 1];
            go.transform.localScale = Vector3.one * 0.25f;
            AplicarMat(go.GetComponent<Renderer>(), matsFlores[i - 1]);
            EditorUtility.SetDirty(go);
            Debug.Log($"✅ Flor_Coletavel_0{i} corrigida");
        }

        // ── Fonte_Principal ──────────────────────────────────────
        var fonte = GameObject.Find("Fonte_Principal");
        if (fonte != null)
        {
            fonte.transform.position   = new Vector3(0f, 0.4f, 0f);
            fonte.transform.localScale = new Vector3(1.5f, 0.4f, 1.5f);
            AplicarMat(fonte.GetComponent<Renderer>(), matFonte);
            EditorUtility.SetDirty(fonte);
            Debug.Log("✅ Fonte_Principal corrigida");
        }

        // ── HUD_Canvas ───────────────────────────────────────────
        var hud = GameObject.Find("HUD_Canvas");
        if (hud != null)
        {
            hud.transform.position   = new Vector3(0f, 1.8f, 3f);
            hud.transform.rotation   = Quaternion.identity;
            hud.transform.localScale = new Vector3(0.003f, 0.003f, 0.003f);
            EditorUtility.SetDirty(hud);
            Debug.Log("✅ HUD_Canvas corrigido");
        }

        // ── XROrigin ─────────────────────────────────────────────
        var xr = GameObject.Find("XROrigin");
        if (xr != null) { xr.transform.position = Vector3.zero; EditorUtility.SetDirty(xr); }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log($"✅ Fix completo! Shader: {shader.name} — Salve com Ctrl+S.");
    }
}
#endif
