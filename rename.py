import os

nameToReplace = "RDTemplate"
newName = "TicketWaveAz"
initialPath = "./"
foldersToIgnore = ["bin", "obj", "debug", ".git", ".vs", "bkp"]
filesToIgnore = [".gitignore", ".gitmodules", ".dockerignore", "rename.py"]


def renameFileData(filename):
    try:
        with open(filename, 'rt', encoding="utf-8") as fin:
            data = fin.read()
    except UnicodeDecodeError:
        try:
            with open(filename, 'rt', encoding="latin-1") as fin:
                data = fin.read()
        except Exception as e:
            print(f"[ERRO] Falha ao ler '{filename}': {e}")
            return

    data = data.replace(nameToReplace, newName)

    try:
        with open(filename, 'wt', encoding="utf-8") as fout:
            fout.write(data)
    except Exception as e:
        print(f"[ERRO] Falha ao escrever '{filename}': {e}")


def renameFileName(path, filename):
    renamed = filename.replace(nameToReplace, newName)
    src = os.path.join(path, filename)
    dst = os.path.join(path, renamed)
    try:
        os.rename(src, dst)
    except Exception as e:
        print(f"[ERRO] Falha ao renomear '{src}' para '{dst}': {e}")


def getInFolder(path):
    for filename in os.listdir(path):
        fileWithPath = os.path.join(path, filename)

        if os.path.isdir(fileWithPath):
            if not any(ignored in filename for ignored in foldersToIgnore):
                getInFolder(fileWithPath)
        else:
            if not any(ignored in filename for ignored in filesToIgnore):
                renameFileData(fileWithPath)

        if nameToReplace in filename:
            renameFileName(path, filename)


def main():
    getInFolder(initialPath)


if __name__ == "__main__":
    main()
