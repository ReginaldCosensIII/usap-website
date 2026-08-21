# USAP Website Repository Instructions

These instructions apply to every agent working in this repository.
Read this file and the referenced project documents before making changes.

## Roles and authority

- The **human project lead** (Reggie, CES) approves scope changes, Git commits/pushes/merges, external communication, CES-server access, and deployment actions.
- The **System Engineer / Planning Agent** defines tasks, acceptance criteria, architecture decisions, and review outcomes.
- The **Implementation Agent** executes only the approved prompt and returns evidence. It does not act outside authorized scope.

Scope authority: the signed contract (when supplied for workspace review), the final fixed 48-hour proposal/estimate, the human-maintained time tracker, and written approvals control working scope.
Until the contract copy is supplied, the proposal, tracker, and written approvals are the controlling documents.

Time and cost tracking is the human lead's responsibility. Actual time is recorded in the approved spreadsheet, not in repository documentation.

## Before editing any file

1. Read the complete implementation prompt.
2. Run `git status --short --branch`, `git remote -v`, and inspect the working tree.
3. Read the applicable documents under `docs/`.
4. Confirm the task is within authorized scope.
5. Stop and report if required information is missing or repository state conflicts with the prompt.

## Change discipline

- Make the smallest coherent change that satisfies the approved task.
- Preserve unrelated work and established patterns.
- Do not perform broad refactors, dependency upgrades, route renames, or architecture changes without explicit approval.
- Do not invent USAP engineering specifications, product claims, customers, testimonials, certifications, military relationships, legal text, or contact details.
- Keep placeholders clearly identified.
- Never commit secrets, credentials, private keys, production configuration, or real customer submissions.

## ASP.NET Core standards

- Use server-rendered Razor Pages and progressive enhancement unless otherwise approved.
- Keep nullable reference types enabled and address compiler warnings within changed code.
- Prefer built-in platform features and minimal external dependencies.
- Use dependency injection deliberately; avoid unnecessary abstraction.
- Use asynchronous APIs for real I/O.
- Validate all form input server-side and preserve antiforgery tokens.
- Encode output by default; do not render untrusted HTML.
- Keep environment-specific values and secrets outside source control.
- Do not add a database, CMS, SPA framework, frontend build system, or cloud service without approval.

## Accessibility, security, and responsive requirements

- Use semantic HTML and native elements before custom controls.
- Provide keyboard access, visible focus indicators, accessible names, correct labels, and ARIA only where needed.
- Design mobile-first. Verify desktop, tablet (≈768 px), and mobile (≈390 px) behavior.
- Do not globally hide horizontal overflow (`overflow-x: hidden`).
- Respect `prefers-reduced-motion`.
- Do not add color-only status indicators, inaccessible modals, or hover-only interactions.
- Do not add JavaScript without approval.

## Validation

At minimum for any code or configuration change:

```powershell
dotnet restore USAP.Web.sln
dotnet build USAP.Web.sln --configuration Debug --no-restore
dotnet build USAP.Web.sln --configuration Release --no-restore
dotnet format USAP.Web.sln --verify-no-changes --no-restore
```

Run the application and verify affected routes. Report exact command output. Never claim a check passed without evidence.

## Documentation — update only materially affected documents

| What changed | Document to update |
|---|---|
| Setup or commands | `README.md` |
| Architecture, configuration, data approach | `docs/ARCHITECTURE.md` |
| Routes, pages, content status | `docs/ROUTES_AND_CONTENT.md` |
| Content status, assets, outstanding client inputs | `docs/CONTENT_AND_ASSETS.md` |
| Consequential decision | `docs/DECISION_LOG.md` |
| Shared design tokens, components | `docs/DESIGN_SYSTEM.md` (create when applicable) |
| Actual redirect entries | `docs/REDIRECT_MAP.md` (create when applicable) |
| IIS / deployment procedure | `docs/DEPLOYMENT_RUNBOOK.md` |
| Operational ownership, known limitations at launch | `docs/HANDOFF.md` |

Do not update every document on every task. Update only documents materially affected by the task.

## Git and external actions

- Do not switch branches, stage, commit, push, open/merge a PR, tag, deploy, send email, or modify external services unless the human lead explicitly authorizes that action in writing.
- Never use destructive Git or filesystem commands without explicit approval.
- Suggest a conventional commit message in the return report.

## Required return report

Every Implementation Agent task returns:

1. Result summary.
2. Repository path, branch, and working-tree status.
3. Exact files changed, created, and deleted.
4. Important decisions and assumptions.
5. Commands run and actual results.
6. Build, format, and route verification evidence.
7. Manual checks remaining for the human lead.
8. Known issues, blockers, or follow-up.
9. Confirmation that no secrets/certificates were added.
10. Confirmation that no staging, commit, push, fetch, pull, deployment, or external mutation occurred.
11. Scope classification.
12. Suggested commit message.
