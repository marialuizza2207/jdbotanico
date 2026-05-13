=====================================================
  JARDIMMETA: JARDIM BOTÂNICO VIRTUAL
  Web 3.0 | Residência em TIC 29 — Unidade 1 / Capítulo 3
  Aluna: Maria Luiza de Moraes Mazon | Prof.: Ana Beatriz
=====================================================

DESCRIÇÃO
---------
Ambiente VR interativo criado em Unity 6 com Meta XR SDK.
O jogador explora um jardim botânico virtual, coleta flores
luminosas e interage com uma fonte d'água central, tudo
controlado pelo teclado no Editor (sem necessidade de headset
para testar).

REPOSITÓRIO GITHUB
------------------
[Link do repositório aqui]

REQUISITOS PARA RODAR
---------------------
- Unity 6000.3.14f1 (ou superior)
- Meta XR SDK (instalado via manifest.json)
- Plataforma: Android (Meta Quest) ou PC (Editor)

COMO ABRIR O PROJETO
--------------------
1. Clone o repositório:
   git clone [link do repositório]

2. Abra o Unity Hub
3. Clique em "Add" e selecione a pasta JardimBotanico/
4. Abra a cena: Assets/Scenes/JardimBotanico.unity
5. Pressione Play para testar no Editor

COMO MONTAR A CENA (primeira vez)
----------------------------------
Após abrir o projeto no Unity, use os menus em ordem:
  1. Tools > Build Jardim Scene        (cria a hierarquia)
  2. Tools > Build Pavilhão            (constrói o pavilhão e árvores)
  3. Tools > Adicionar Componentes do Jardim  (adiciona scripts e coliders)
  4. Tools > Fix Jardim                (corrige materiais e posições)
  5. Ctrl+S                            (salva a cena)

CONTROLES (TECLADO/MOUSE)
--------------------------
W / Seta Cima    → Mover para frente
S / Seta Baixo   → Mover para trás
A / Seta Esq     → Mover para esquerda
D / Seta Dir     → Mover para direita
E ou Clique      → Ativar fonte (ao se aproximar)

MECÂNICAS
---------
- Aproximar das flores coloridas  → coletar (+pontos)
- Aproximar da fonte central      → hover (cor ciano)
- Pressionar E perto da fonte     → ativar (azul escuro)
- HUD mostra pontuação e progresso de flores em tempo real

HIERARQUIA DA CENA
------------------
[--- MANAGEMENT ---]
  GerenciadorJardim  → controle de pontuação e progresso
  EventSystem        → sistema de eventos UI
  HUD_Canvas         → interface do jogador (World Space)
    Texto_Pontuacao
    Texto_Flores
    Texto_Mensagem

[--- PLAYER ---]
  XROrigin           → câmera e movimentação VR/PC

[--- ENVIRONMENT ---]
  Plano_Gramado      → terreno navegável (gramado verde)
  Directional Light  → iluminação principal
  Pavilhao           → estrutura central do jardim
    Colunas
      Coluna_FE / Coluna_FD / Coluna_TE / Coluna_TD
    Telhado
      Telhado_Mesh
  Banco              → banco de jardim
    Assento
    Pes
  Arvores            → três árvores decorativas
    Arvore_01 / Arvore_02 / Arvore_03
      Tronco / Copa
  Poste_Luz          → poste com Point Light

[--- INTERACTABLES ---]
  Flor_Coletavel_01  (Rosa      — 10 pts)
  Flor_Coletavel_02  (Tulipa    — 15 pts)
  Flor_Coletavel_03  (Orquídea  — 25 pts)
  Flor_Coletavel_04  (Girassol  — 20 pts)
  Flor_Coletavel_05  (Lavanda   — 30 pts)
  Fonte_Principal    → interação por proximidade

ESTRUTURA DE PASTAS
-------------------
Assets/
  Scripts/    → GerenciadorJardim, JogadorController, HUDJardim,
                FlorescenteController/View,
                FonteInterativaController/View
  Editor/     → JardimBuilder, PavilhaoBuilder,
                AdicionarComponentes, AutoConectarReferencias,
                FixJardim
  Materials/  → Materiais coloridos (gerados pelo FixJardim)
  Scenes/     → JardimBotanico.unity
  Prefabs/    → Prefabs do Meta XR SDK
ProjectSettings/
Packages/

CONFIGURAÇÃO DE BUILD
---------------------
- Platform: Android
- Minimum API Level: Android 10 (Level 29)
- Texture Compression: ASTC
- XR Plugin: OVR (Meta/Oculus)

=====================================================
