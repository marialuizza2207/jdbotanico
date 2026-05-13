# JardimMeta — Jardim Botânico Virtual

> **Web 3.0 | Residência em TIC 29 — Unidade 1 / Capítulo 3**
> Aluna: Maria Luiza de Moraes Mazon | Professora: Ana Beatriz

Ambiente VR interativo criado em Unity 6 com Meta XR SDK. O jogador explora um jardim botânico virtual, coleta cinco flores luminosas e interage com uma fonte d'água central. Totalmente testável no Unity Editor via teclado, sem necessidade de headset.

---

## Requisitos

| Item | Versão |
|---|---|
| Unity | 6000.3.14f1 |
| Meta XR SDK Core | 201.0.0 |
| Meta XR SDK Interaction | 201.0.0 |
| XR Interaction Toolkit | 3.0.7 |
| TextMesh Pro | 3.0.6 |
| Plataforma de build | Android (Meta Quest) |

> As dependências são instaladas automaticamente via `Packages/manifest.json`.

---

## Como abrir o projeto

```bash
git clone https://github.com/marialuizza2207/jdbotanico.git
```

1. Abra o **Unity Hub**
2. Clique em **Add → Add project from disk** e selecione a pasta `JardimBotanico/`
3. Aguarde o Unity importar os pacotes (pode levar alguns minutos na primeira vez)
4. Abra a cena: `Assets/Scenes/JardimBotanico.unity`

---

## Como montar a cena (primeira vez)

Execute os menus **Tools** nesta ordem após abrir o projeto:

| Ordem | Menu | O que faz |
|---|---|---|
| 1 | **Tools → Build Jardim Scene** | Cria toda a hierarquia de GameObjects |
| 2 | **Tools → Build Pavilhão** | Constrói o pavilhão, árvores e poste |
| 3 | **Tools → Adicionar Componentes do Jardim** | Adiciona scripts, colliders e câmera |
| 4 | **Tools → Auto-Conectar Referências do Jardim** | Liga GerenciadorJardim → HUD → textos |
| 5 | **Tools → Fix Jardim** | Aplica materiais coloridos e corrige posições |
| 6 | **Ctrl + S** | Salva a cena |

Depois pressione **Play** para testar.

---

## Controles (teclado)

| Tecla | Ação |
|---|---|
| `W` / `↑` | Mover para frente |
| `S` / `↓` | Mover para trás |
| `A` / `←` | Mover para esquerda |
| `D` / `→` | Mover para direita |
| `E` | Ativar a Fonte (ao se aproximar) |

---

## Mecânicas

- **Aproximar de uma flor** → coleta automática por proximidade (`Physics.OverlapSphere`) e acumula pontos
- **Aproximar da fonte** → cor muda para ciano (hover)
- **Pressionar E perto da fonte** → fonte ativada (cor azul escuro)
- **HUD World Space** → exibe pontuação, progresso `Flores: X/5` e mensagens em tempo real, flutuando à frente da câmera

---

## Hierarquia da Cena

```
JardimBotanico (Scene)
│
├── [--- MANAGEMENT ---]
│   ├── GerenciadorJardim       ← controla pontuação e progresso
│   ├── EventSystem
│   └── HUD_Canvas              ← Canvas World Space (segue a câmera)
│       ├── Texto_Pontuacao
│       ├── Texto_Flores
│       └── Texto_Mensagem
│
├── [--- PLAYER ---]
│   └── XROrigin                ← tag: Player | JogadorController
│       └── Main Camera         ← câmera filha (altura dos olhos: 1.7 m)
│
├── [--- ENVIRONMENT ---]
│   ├── Plano_Gramado           ← gramado verde navegável
│   ├── Directional Light       ← luz solar dourada (45°, intensidade 1.2)
│   ├── Pavilhao                ← estrutura central do jardim
│   │   ├── Colunas
│   │   │   ├── Coluna_FE / Coluna_FD
│   │   │   └── Coluna_TE / Coluna_TD
│   │   └── Telhado → Telhado_Mesh
│   ├── Banco
│   │   ├── Assento → Assento_Mesh
│   │   └── Pes
│   ├── Arvores
│   │   ├── Arvore_01 → Tronco / Copa
│   │   ├── Arvore_02 → Tronco / Copa
│   │   └── Arvore_03 → Tronco / Copa
│   └── Poste_Luz
│       ├── Haste → Haste_Mesh
│       └── Lampada             ← Point Light, alcance 8 m
│
└── [--- INTERACTABLES ---]
    ├── Flor_Coletavel_01       ← Rosa       — 10 pts
    ├── Flor_Coletavel_02       ← Tulipa     — 15 pts
    ├── Flor_Coletavel_03       ← Orquídea   — 25 pts
    ├── Flor_Coletavel_04       ← Girassol   — 20 pts
    ├── Flor_Coletavel_05       ← Lavanda    — 30 pts
    └── Fonte_Principal         ← interação por proximidade
```

---

## Estrutura de Pastas

```
JardimBotanico/
├── Assets/
│   ├── Scripts/        ← runtime: GerenciadorJardim, HUDJardim, JogadorController,
│   │                              FlorescenteController/View, FonteInterativaController/View
│   ├── Editor/         ← tools: JardimBuilder, PavilhaoBuilder,
│   │                            AdicionarComponentes, AutoConectarReferencias, FixJardim
│   ├── Materials/      ← materiais gerados pelo FixJardim
│   ├── Scenes/         ← JardimBotanico.unity
│   ├── Prefabs/
│   └── Resources/
├── Packages/
│   └── manifest.json   ← dependências (Meta XR SDK, XRI, TMP, Input System)
└── ProjectSettings/
```

---

## Scripts

### Runtime (`Assets/Scripts/`)

| Script | Responsabilidade |
|---|---|
| `GerenciadorJardim.cs` | Estado global: pontuação e contagem de flores coletadas |
| `HUDJardim.cs` | HUD World Space — segue a câmera no `LateUpdate` via TextMeshPro |
| `JogadorController.cs` | Movimentação WASD + detecção por `Physics.OverlapSphere` |
| `FlorescenteController.cs` | Lógica de coleta: registra no GerenciadorJardim e aciona a View |
| `FlorescenteView.cs` | Visual: oscilação senoidal e rotação contínua da flor |
| `FonteInterativaController.cs` | Lógica de hover/ativação da fonte; compatível com XRSimpleInteractable |
| `FonteInterativaView.cs` | Visual: mudança de cor da fonte conforme estado |

### Editor (`Assets/Editor/`)

| Script | Menu | Função |
|---|---|---|
| `JardimBuilder.cs` | Tools → Build Jardim Scene | Cria a hierarquia completa da cena |
| `PavilhaoBuilder.cs` | Tools → Build Pavilhão | Constrói pavilhão, árvores e poste com meshes |
| `AdicionarComponentes.cs` | Tools → Adicionar Componentes do Jardim | Adiciona scripts, colliders e câmera |
| `AutoConectarReferencias.cs` | Tools → Auto-Conectar Referências do Jardim | Conecta referências entre componentes via código |
| `FixJardim.cs` | Tools → Fix Jardim | Cria e aplica materiais coloridos, corrige posições |

---

## Configuração de Build (Android / Meta Quest)

1. **File → Build Settings → Android → Switch Platform**
2. **Player Settings:**
   - Company Name: `MariaLuizaMazon`
   - Product Name: `JardimMeta`
   - Minimum API Level: Android 10 (Level 29)
   - Texture Compression: ASTC
3. **Project Settings → XR Plugin Management → Android:** marcar **Oculus**

---

## Arquivos ignorados pelo Git

Certifique-se de que o `.gitignore` exclui:

```
/Library/
/Temp/
/Obj/
/Build/
/Logs/
/UserSettings/
*.csproj
*.sln
```

---

*Web 3.0 | Residência em TIC 29 — Maria Luiza de Moraes Mazon*
