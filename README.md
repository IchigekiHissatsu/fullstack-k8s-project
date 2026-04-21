# E2E Automatizált Microservice Projekt (Angular 21 & .NET 10)

Ez a projekt egy professzionális, automatizált, Kubernetes alapú microservice architektúra, amely a fejlesztéstől a telepítésig (CI/CD) terjedő folyamatot mutatja be.

## Technológiai Stack
- Frontend: Angular 21 (Tailwind CSS v4)
- Backend: ASP.NET 10 Web API (Microservice architektúra)
- Adatbázis: MongoDB 8
- Infrastruktúra: Kubernetes (Docker Desktop local cluster)
- CI: GitHub Actions (ghcr.io registry)
- CD: ArgoCD (GitOps szemlélet)
- Extra: Saját MCP (Model Context Protocol) szerver implementáció

---

## Telepítési útmutató (Installation Guide)

### 1. Előfeltételek
A futtatáshoz szükséges eszközök:
- Docker Desktop (bekapcsolt Kubernetes-szel)
- kubectl parancssori kliens
- Helm

### 2. Namespace és Adatbázis indítása
Futtassa az alábbi parancsokat a környezet kialakításához:

kubectl create namespace my-app-space

helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update
helm install mongodb bitnami/mongodb --namespace my-app-space --set auth.enabled=false

### 3. ArgoCD (CD Workflow) konfigurálása
A CD folyamat az ArgoCD-re épül:

kubectl create namespace argocd
kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml

# Az ArgoCD elérése (Port-forward után: https://localhost:8080)
kubectl port-forward svc/argocd-server -n argocd 8080:443

Megjegyzés: Az admin jelszó a következő paranccsal nyerhető ki:
kubectl -n argocd get secret argocd-initial-admin-secret -o jsonpath="{.data.password}"

---

## User Guide (Használati útmutató)

### Alkalmazás elérése
- Webes felület: http://localhost
- API tesztelés: A backend/MyBackendAPI mappában található api_tests.http fájllal tesztelhetők a CRUD műveletek (VS Code REST Client ajánlott).

### Funkciók
- Könyvtár kezelés: Teljes CRUD támogatás MongoDB adatbázissal.
- Pagination: Az Angular frontend 3 elemes lapozást biztosít.
- Microservices: Szétválasztott komponensek (Frontend, API, Inventory Service), melyek a klaszteren belül kommunikálnak.
- MCP Szerver: Node.js alapú szerver, amely az AI modellek számára biztosít strukturált adatérést.

---

## CI/CD Automatizáció

A rendszer a GitOps elveket követi:
1. CI: A GitHubra való push indítja a GitHub Actionst, ami buildeli és feltölti az image-eket (ghcr.io).
2. CD: Az ArgoCD figyeli a /k8s mappát, és változás esetén automatikusan szinkronizálja a klasztert.

---
Készítette: UOZ4OC