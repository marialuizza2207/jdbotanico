using UnityEngine;

public class GerenciadorJardim : MonoBehaviour
{
    public HUDJardim hudJardim;

    private int pontuacao = 0;
    private int totalFlores = 5;
    private int floresColetadas = 0;

    private void Start()
    {
        if (hudJardim != null)
        {
            hudJardim.AtualizarPontuacao(pontuacao);
            hudJardim.AtualizarFlores(floresColetadas, totalFlores);
            hudJardim.ExibirMensagem("Explore o jardim e colete as flores!");
        }
    }

    public void RegistrarColeta(int pontos)
    {
        pontuacao += pontos;
        floresColetadas++;

        if (hudJardim != null)
        {
            hudJardim.AtualizarPontuacao(pontuacao);
            hudJardim.AtualizarFlores(floresColetadas, totalFlores);

            if (floresColetadas >= totalFlores)
                hudJardim.ExibirMensagem("Parabéns! Jardim completo! Você coletou todas as flores!");
            else
                hudJardim.ExibirMensagem("Flor coletada! +" + pontos + " pontos");
        }
    }
}
