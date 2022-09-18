CREATE procedure [dbo].[spGetAllVideoFile]    
as    
begin    
select VideoId,Name,FileSize,FilePath from Videos    
end 