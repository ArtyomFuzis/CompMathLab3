using NotAWitchMat3;

while (true)
{
    string? ln = Console.ReadLine();
    if (ln == "") continue;
    if (ln == null || ln[0] == 0x04)
    {
        break;
    }
    Commands.process_command(ln.Trim());
}
