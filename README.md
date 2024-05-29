# kubec-cmd

## Info
Manage and rotate Kubernetes configuration files in the .kube folder with efficient backups and changes.

## Program overview

kubec-cmd aims to help users rotate and manage Kubernetes configuration files in the `.kube` folder.


**Note**: Make sure you have the `config_main`, `config_local`, `config_remote`, etc., files in the `.kube` folder before running the commands above.

## How to use

Assuming you have the following files in `.kube`:

- `./kube/config_main`
- `./kube/config_repo`

To set the `config_repo` file as the active configuration, you can use the following command:

```bash
kubec-cmd -t repo
```

This will search for the file:
./kube/config_repo


And set it as the active configuration file used by kubectl.

In summary,  kubec-cmd allows users to easily switch between different Kubernetes configuration files, making the configuration file rotation process more efficient and secure.
