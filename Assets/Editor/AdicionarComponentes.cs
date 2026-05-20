#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class AdicionarComponentes : EditorWindow
{
    [MenuItem("Tools/Adicionar Componentes do Jardim")]
    public static void AddComponents()
    {
        
        var gjGO = GameObject.Find("GerenciadorJardim");
        if (gjGO != null)
        {
            AddIfMissing<GerenciadorJardim>(gjGO);
        }

        
        var esGO = GameObject.Find("EventSystem");
        if (esGO != null)
        {
            AddIfMissing<EventSystem>(esGO);
            AddIfMissing<StandaloneInputModule>(esGO);
        }

        // HUD_Canvas
        var hudGO = GameObject.Find("HUD_Canvas");
        if (hudGO != null)
        {
            var canvas = AddIfMissing<Canvas>(hudGO);
            canvas.renderMode = RenderMode.WorldSpace;
            AddIfMissing<CanvasScaler>(hudGO);
            AddIfMissing<GraphicRaycaster>(hudGO);
            AddIfMissing<HUDJardim>(hudGO);

            hudGO.transform.position   = new Vector3(0f, 1.6f, 2f);
            hudGO.transform.localScale = new Vector3(0.002f, 0.002f, 0.002f);

            SetupTMP("Texto_Pontuacao", "Pontos: 0");
            SetupTMP("Texto_Flores",   "Flores: 0/5");
            SetupTMP("Texto_Mensagem", "Explore o jardim e colete as flores!");

            EditorUtility.SetDirty(hudGO);
            // ok;
        }

        // XROrigin / JogadorController
        var xrGO = GameObject.Find("XROrigin");
        if (xrGO != null)
        {
            AddIfMissing<JogadorController>(xrGO);
            xrGO.tag = "Player";

            // Garante câmera filha do player (se não existir, cria)
            var camFilha = xrGO.GetComponentInChildren<Camera>();
            if (camFilha == null)
            {
                var camPadrao = GameObject.Find("Main Camera");
                if (camPadrao != null) Object.DestroyImmediate(camPadrao);

                var camObj = new GameObject("Main Camera");
                camObj.transform.SetParent(xrGO.transform);
                camObj.transform.localPosition = new Vector3(0f, 1.7f, 0f);
                camObj.transform.localRotation = Quaternion.identity;
                var cam = camObj.AddComponent<Camera>();
                cam.tag = "MainCamera";
                if (camObj.GetComponent<AudioListener>() == null)
                    camObj.AddComponent<AudioListener>();
                EditorUtility.SetDirty(camObj);
                // camera criada;
            }

            EditorUtility.SetDirty(xrGO);
        }

        // Flores Coletáveis
        string[] nomesFlores  = { "Rosa", "Tulipa", "Orquidea", "Girassol", "Lavanda" };
        int[]    pontosFlores  = { 10, 15, 25, 20, 30 };

        for (int i = 1; i <= 5; i++)
        {
            var go = GameObject.Find($"Flor_Coletavel_0{i}");
            if (go == null) continue;

            AddIfMissing<SphereCollider>(go);
            AddIfMissing<FlorescenteView>(go);
            var ctrl = AddIfMissing<FlorescenteController>(go);
            ctrl.nomeFlor = nomesFlores[i - 1];
            ctrl.pontos   = pontosFlores[i - 1];

            var xrInt = AddIfMissing<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>(go);
            xrInt.selectEntered.AddListener(ctrl.OnInteracaoXR);

            EditorUtility.SetDirty(go);
            // ok;
        }

        // Fonte_Principal
        var fonteGO = GameObject.Find("Fonte_Principal");
        if (fonteGO != null)
        {
            AddIfMissing<CapsuleCollider>(fonteGO);
            AddIfMissing<FonteInterativaView>(fonteGO);
            var fCtrl = AddIfMissing<FonteInterativaController>(fonteGO);

            var xrInt = AddIfMissing<UnityEngine.XR.Interaction.Toolkit.Interactables.XRSimpleInteractable>(fonteGO);
            xrInt.hoverEntered.AddListener(fCtrl.AoEntrarHover);
            xrInt.hoverExited.AddListener(fCtrl.AoSairHover);
            xrInt.selectEntered.AddListener(fCtrl.AoAtivar);

            EditorUtility.SetDirty(fonteGO);
            // ok;
        }

        
        AutoConectarReferencias.ConectarReferencias();

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        Debug.Log("[AdicionarComponentes] Concluido. Salve com Ctrl+S.");
    }

    static T AddIfMissing<T>(GameObject go) where T : Component
    {
        var comp = go.GetComponent<T>();
        if (comp == null) comp = go.AddComponent<T>();
        return comp;
    }

    static void SetupTMP(string goName, string textoInicial)
    {
        var go = GameObject.Find(goName);
        if (go == null) return;
        var tmp = go.GetComponent<TextMeshProUGUI>();
        if (tmp == null) tmp = go.AddComponent<TextMeshProUGUI>();
        tmp.text      = textoInicial;
        tmp.fontSize  = 36;
        tmp.alignment = TextAlignmentOptions.Center;
        var rect = go.GetComponent<RectTransform>();
        if (rect) rect.sizeDelta = new Vector2(300f, 60f);
        EditorUtility.SetDirty(go);
    }
}
#endif
