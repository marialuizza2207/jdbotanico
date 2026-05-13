using UnityEngine;
using UnityEngine.InputSystem;

public class JogadorController : MonoBehaviour
{
    [SerializeField] public Camera referenciaCamera;
    [SerializeField] float velocidade = 3f;
    [SerializeField] float raioColeta = 1.5f;
    [SerializeField] float raioFonte  = 1.2f;

    private FonteInterativaController fonteAtual = null;

    void Update()
    {
        float h = 0f, v = 0f;

        var kb = Keyboard.current;
        if (kb != null)
        {
            if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) h += 1f;
            if (kb.aKey.isPressed || kb.leftArrowKey.isPressed)  h -= 1f;
            if (kb.wKey.isPressed || kb.upArrowKey.isPressed)    v += 1f;
            if (kb.sKey.isPressed || kb.downArrowKey.isPressed)  v -= 1f;
        }

        transform.Translate(new Vector3(h, 0f, v) * velocidade * Time.deltaTime, Space.Self);

        // Coleta de flores por proximidade
        foreach (var col in Physics.OverlapSphere(transform.position, raioColeta))
        {
            var ctrl = col.GetComponent<FlorescenteController>();
            if (ctrl != null) ctrl.TentarColetar();
        }

        // Hover da fonte por proximidade
        FonteInterativaController fonteEncontrada = null;
        foreach (var col in Physics.OverlapSphere(transform.position, raioFonte))
        {
            var f = col.GetComponent<FonteInterativaController>();
            if (f != null) { fonteEncontrada = f; break; }
        }

        if (fonteEncontrada != null && fonteAtual == null)
        {
            fonteAtual = fonteEncontrada;
            fonteAtual.AoEntrarHoverProximidade();
        }
        else if (fonteEncontrada == null && fonteAtual != null)
        {
            fonteAtual.AoSairHoverProximidade();
            fonteAtual = null;
        }

        if (fonteAtual != null && kb != null && kb.eKey.wasPressedThisFrame)
            fonteAtual.AoAtivarProximidade();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, raioColeta);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, raioFonte);
    }
}
