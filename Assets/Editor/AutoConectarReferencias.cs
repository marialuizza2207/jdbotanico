#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using TMPro;

public class AutoConectarReferencias : EditorWindow
{
    [MenuItem("Tools/Auto-Conectar Referências do Jardim")]
    public static void ConectarReferencias()
    {
        var gj  = Object.FindObjectOfType<GerenciadorJardim>();
        var hud = Object.FindObjectOfType<HUDJardim>();

        if (gj && hud)
        {
            gj.hudJardim = hud;
            EditorUtility.SetDirty(gj);
            // ok;
        }

        if (hud)
        {
            var tp = GameObject.Find("Texto_Pontuacao");
            var tf = GameObject.Find("Texto_Flores");
            var tm = GameObject.Find("Texto_Mensagem");
            if (tp) hud.textoPontuacao = tp.GetComponent<TextMeshProUGUI>();
            if (tf) hud.textoFlores    = tf.GetComponent<TextMeshProUGUI>();
            if (tm) hud.textoMensagem  = tm.GetComponent<TextMeshProUGUI>();
            EditorUtility.SetDirty(hud);
            // ok;
        }

        var jc  = Object.FindObjectOfType<JogadorController>();
        var cam = Camera.main;
        if (jc && cam)
        {
            jc.referenciaCamera = cam;
            EditorUtility.SetDirty(jc);
            // ok;
        }

        string[] nomesFlores = { "Rosa", "Tulipa", "Orquidea", "Girassol", "Lavanda" };
        int[]    pontosFlores = { 10, 15, 25, 20, 30 };
        for (int i = 0; i < 5; i++)
        {
            var go = GameObject.Find($"Flor_Coletavel_0{i + 1}");
            if (go == null) continue;
            var ctrl = go.GetComponent<FlorescenteController>();
            if (ctrl == null) continue;
            ctrl.nomeFlor = nomesFlores[i];
            ctrl.pontos   = pontosFlores[i];
            EditorUtility.SetDirty(go);
            // ok;
        }

        UnityEditor.SceneManagement.EditorSceneManager.MarkAllScenesDirty();
        // ok;
    }
}
#endif
