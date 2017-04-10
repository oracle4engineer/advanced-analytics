Sub 日本語列名の修正()
    
    Set sheet_cur = ActiveSheet
    str_sheet_bak_name = sheet_cur.Name + "_元の列名"
    
    For Each ws In Worksheets
        If ws.Name = str_sheet_bak_name Then
            MsgBox "ワークシート「" + str_sheet_bak_name + "」を削除してください", vbInformation
            ws.Delete
        End If
    Next ws

    Set sheet_bak = Sheets.Add(After:=sheet_cur)
    sheet_bak.Name = str_sheet_bak_name

    With sheet_cur

        ' 元の列名をバックアップしておく
        .Rows(1).Copy
        sheet_bak.Rows(1).Select
        sheet_bak.Paste
        .Select

        ' 行列範囲（1行目と1列目は埋まっている前提）
        col_end = .Rows(1).End(xlToRight).Column
        row_end = .Columns(1).End(xlDown).Row

        For col_cur = 1 To col_end

            ' 変換前のカラム名を取得
            str_name = .Cells(1, col_cur).Value
            
            ' 半角ダブルクォーテーションを削除する
            str_name = Replace(str_name, """", "")
            
            ' DBCS関数で全ての英数字とカタカナを大文字にする
            str_name = WorksheetFunction.Dbcs(str_name)
            
            ' 全角スペースは紛らわしいので削除する
            str_name = Replace(str_name, "　", "")
            
            ' 26バイトを超えている場合には26バイトにする
            If LenB(str_name) > 26 Then
                str_name = LeftB(str_name, 26)
            End If
            
            '列の通し番号を列名の前につける
            str_name = Format(col_cur, "000") + "_" + str_name

            ' ダブルクォーテーションでくくる
            .Cells(1, col_cur).Value = """" + str_name + """"

        Next
    End With
End Sub

Sub 表のデータの変更()
    
    Set sheet_cur = ActiveSheet
    str_sheet_bak_name = sheet_cur.Name + "_元のデータ"
    
    For Each ws In Worksheets
        If ws.Name = str_sheet_bak_name Then
            MsgBox "ワークシート「" + str_sheet_bak_name + "」を削除してください", vbInformation
            ws.Delete
        End If
    Next ws

    Set sheet_bak = Sheets.Add(After:=sheet_cur)
    sheet_bak.Name = str_sheet_bak_name

    With sheet_cur

        ' 元のデータをバックアップしておく
        .Cells.Copy
        sheet_bak.Select
        sheet_bak.Paste
        .Select

        ' 行列範囲（1行目と1列目は埋まっている前提）
        col_end = .Rows(1).End(xlToRight).Column
        row_end = .Columns(1).End(xlDown).Row

        With Range(Cells(2, 1), Cells(row_end, col_end))
        
            ' 半角スペースの削除
            .Replace What:=" ", Replacement:=""
            
            ' 空欄を0で埋める
            .Replace What:="", Replacement:="0"
            
        End With
    
    End With

End Sub

