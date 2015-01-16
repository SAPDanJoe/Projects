Module Module1

    Sub Main()
        Dim wait As String
        Dim cs As String
        Dim strCommand As String

        cs = "Persist Security Info=False;Integrated Security=true;Server=OAKD00469894A\SQL_EUS_OAK;Database=Import;User Id=user;Password=kPdnpZn0603;"
        strCommand = "Execute HuntBuildingCodes"

        Using connection As New SqlClient.SqlConnection(cs)
            Dim command As New SqlClient.SqlCommand(strCommand, connection)

            Try
                connection.Open()
                Dim rowsAffected As Integer = Command.ExecuteNonQuery()
                Console.WriteLine("RowsAffected: {0}", rowsAffected)

                Console.WriteLine("Done With 'Try'")
                wait = Console.ReadLine()

            Catch ex As Exception
                Console.WriteLine("exception is " + ex.Message.ToString)
                wait = Console.ReadLine()
            End Try
        End Using
    End Sub


End Module
