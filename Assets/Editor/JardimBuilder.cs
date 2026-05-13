#if UNITY_EDITOR
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Cria toda a hierarquia da cena via menu Tools/Build Jardim Scene
public class JardimBuilder : EditorWindow
{
    [MenuItem("Tools/Build Jardim Scene")]
    public static void ConstruirCena()
    {
        CriarManagement();
        CriarJogador();
        CriarAmbiente();
        CriarInterativos();

        Debug.Log("[JardimBuilder] Hierarquia do jardim criada com sucesso!");
    }

    // ── [--- MANAGEMENT ---] ────────────────────────────────────

    private static void CriarManagement()
    {
        GameObject raiz = new GameObject("[--- MANAGEMENT ---]");

        GameObject gjObj = new GameObject("GerenciadorJardim");
        gjObj.transform.SetParent(raiz.transform);
        gjObj.AddComponent<GerenciadorJardim>();

        GameObject esObj = new GameObject("EventSystem");
        esObj.transform.SetParent(raiz.transform);
        esObj.AddComponent<EventSystem>();
        esObj.AddComponent<StandaloneInputModule>();

        GameObject canvasObj = new GameObject("HUD_Canvas");
        canvasObj.transform.SetParent(raiz.transform);
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>();

        canvasObj.transform.localPosition = new Vector3(0f, 1.5f, 2f);
        canvasObj.transform.localScale    = Vector3.one * 0.01f;

        CriarTextoTMP(canvasObj, "Texto_Pontuacao", new Vector2(0,  60), "Pontuação: 0");
        CriarTextoTMP(canvasObj, "Texto_Flores",    new Vector2(0,   0), "Flores: 0/5");
        CriarTextoTMP(canvasObj, "Texto_Mensagem",  new Vector2(0, -60), "Bem-vindo ao Jardim!");

        canvasObj.AddComponent<HUDJardim>();
    }

    private static GameObject CriarTextoTMP(GameObject pai, string nome, Vector2 posicao, string conteudo)
    {
        GameObject obj = new GameObject(nome);
        obj.transform.SetParent(pai.transform, false);

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text      = conteudo;
        tmp.fontSize  = 36;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.color     = Color.white;

        RectTransform rt = obj.GetComponent<RectTransform>();
        rt.anchoredPosition = posicao;
        rt.sizeDelta        = new Vector2(400, 50);

        return obj;
    }

    // ── [--- PLAYER ---] ────────────────────────────────────────

    private static void CriarJogador()
    {
        // Remove câmera solta que Unity cria por padrão na cena
        var camPadrao = GameObject.Find("Main Camera");
        if (camPadrao != null) Object.DestroyImmediate(camPadrao);

        GameObject raiz = new GameObject("[--- PLAYER ---]");

        GameObject xrOrigin = new GameObject("XROrigin");
        xrOrigin.transform.SetParent(raiz.transform);
        xrOrigin.transform.position = Vector3.zero;
        xrOrigin.tag = "Player";
        xrOrigin.AddComponent<JogadorController>();

        // Câmera filha do player para que siga o movimento
        GameObject camObj = new GameObject("Main Camera");
        camObj.transform.SetParent(xrOrigin.transform);
        camObj.transform.localPosition = new Vector3(0f, 1.7f, 0f);
        camObj.transform.localRotation = Quaternion.identity;
        Camera cam = camObj.AddComponent<Camera>();
        cam.tag = "MainCamera";
        camObj.AddComponent<AudioListener>();
    }

    // ── [--- ENVIRONMENT ---] ────────────────────────────────────

    private static void CriarAmbiente()
    {
        GameObject raiz = new GameObject("[--- ENVIRONMENT ---]");

        // Gramado
        GameObject gramado = GameObject.CreatePrimitive(PrimitiveType.Plane);
        gramado.name = "Plano_Gramado";
        gramado.transform.SetParent(raiz.transform);
        gramado.transform.localScale = new Vector3(5f, 1f, 5f);

        // Luz direcional
        GameObject luz = new GameObject("Directional Light");
        luz.transform.SetParent(raiz.transform);
        Light comp = luz.AddComponent<Light>();
        comp.type      = LightType.Directional;
        comp.intensity = 1.2f;
        comp.color     = new Color(1f, 0.95f, 0.85f);
        luz.transform.rotation = Quaternion.Euler(45f, -35f, 0f);

        // Pavilhão
        GameObject pavilhao = new GameObject("Pavilhao");
        pavilhao.transform.SetParent(raiz.transform);

        GameObject colunas = new GameObject("Colunas");
        colunas.transform.SetParent(pavilhao.transform);

        GameObject telhado = new GameObject("Telhado");
        telhado.transform.SetParent(pavilhao.transform);

        // Banco
        GameObject banco = new GameObject("Banco");
        banco.transform.SetParent(raiz.transform);

        GameObject assento = new GameObject("Assento");
        assento.transform.SetParent(banco.transform);

        GameObject pes = new GameObject("Pes");
        pes.transform.SetParent(banco.transform);

        // Árvores
        GameObject arvores = new GameObject("Arvores");
        arvores.transform.SetParent(raiz.transform);

        for (int i = 1; i <= 3; i++)
        {
            GameObject arvore = new GameObject($"Arvore_0{i}");
            arvore.transform.SetParent(arvores.transform);

            GameObject tronco = new GameObject("Tronco");
            tronco.transform.SetParent(arvore.transform);

            GameObject copa = new GameObject("Copa");
            copa.transform.SetParent(arvore.transform);
        }

        // Poste de Luz
        GameObject poste = new GameObject("Poste_Luz");
        poste.transform.SetParent(raiz.transform);

        GameObject haste = new GameObject("Haste");
        haste.transform.SetParent(poste.transform);

        GameObject lampada = new GameObject("Lampada");
        lampada.transform.SetParent(poste.transform);
    }

    // ── [--- INTERACTABLES ---] ──────────────────────────────────

    private static void CriarInterativos()
    {
        GameObject raiz = new GameObject("[--- INTERACTABLES ---]");

        // Cinco flores coletáveis
        string[] nomes = { "Rosa", "Tulipa", "Orquidea", "Girassol", "Lavanda" };
        Vector3[] posicoes = {
            new Vector3(-3f,  0.5f,  2f),
            new Vector3( 3f,  0.5f,  2f),
            new Vector3(-3f,  0.5f, -2f),
            new Vector3( 3f,  0.5f, -2f),
            new Vector3( 0f,  0.5f,  4f),
        };

        for (int i = 0; i < 5; i++)
        {
            GameObject flor = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            flor.name = $"Flor_Coletavel_0{i + 1}";
            flor.transform.SetParent(raiz.transform);
            flor.transform.position   = posicoes[i];
            flor.transform.localScale = Vector3.one * 0.25f;
            flor.AddComponent<FlorescenteView>();
            flor.AddComponent<FlorescenteController>();
        }

        // Fonte interativa (cilindro achatado)
        GameObject fonte = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        fonte.name = "Fonte_Principal";
        fonte.transform.SetParent(raiz.transform);
        fonte.transform.position   = new Vector3(0f, 0.4f, 0f);
        fonte.transform.localScale = new Vector3(1.5f, 0.4f, 1.5f);
        fonte.AddComponent<FonteInterativaView>();
        fonte.AddComponent<FonteInterativaController>();
    }
}
#endif
