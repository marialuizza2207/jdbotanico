# JardimMeta — Jardim Botânico Virtual

> **Web 3.0 | Residência em TIC 29 — Unidade 1 / Capítulo 3**
> Aluna: Maria Luiza de Moraes Mazon | Professora: Ana Beatriz

Esse projeto é um jardim botânico virtual feito em Unity 6 com Meta XR SDK. Dá pra explorar o espaço, coletar flores e interagir com a fonte de água. Funciona no Unity Editor com teclado — não precisa de headset pra testar.

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

A cena está dividida em quatro grupos de objetos:

- **[--- MANAGEMENT ---]**: GerenciadorJardim, EventSystem e HUD_Canvas (Canvas World Space com os três textos de pontuação/progresso/mensagem)
- **[--- PLAYER ---]**: XROrigin (tag Player, tem o JogadorController) com Main Camera filha na altura dos olhos
- **[--- ENVIRONMENT ---]**: Plano_Gramado, Directional Light, Pavilhao (com 4 colunas e telhado), Banco, três Arvores com Tronco e Copa, e Poste_Luz com Point Light de alcance 8 m
- **[--- INTERACTABLES ---]**: cinco flores coletáveis (Rosa 10 pts, Tulipa 15, Orquídea 25, Girassol 20, Lavanda 30) e Fonte_Principal

---

## Estrutura de Pastas

- `Assets/Scripts/` — scripts de runtime: GerenciadorJardim, HUDJardim, JogadorController, FlorescenteController/View, FonteInterativaController/View
- `Assets/Editor/` — ferramentas de setup: JardimBuilder, PavilhaoBuilder, AdicionarComponentes, AutoConectarReferencias, FixJardim
- `Assets/Materials/` — materiais criados pelo FixJardim
- `Assets/Scenes/` — JardimBotanico.unity
- `Packages/manifest.json` — dependências (Meta XR SDK, XRI, TMP, Input System)
- `ProjectSettings/` — configurações do projeto

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
