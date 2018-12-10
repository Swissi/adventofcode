def get_next(arr, start_index)
  children = arr[start_index]
  start_index += 1
  meta = arr[start_index]
  start_index += 1

  (1..children).each do
    start_index = get_next(arr, start_index)
  end

  (1..meta).each do
    @@sum += arr[start_index]
    start_index += 1
  end

  start_index
end

@@sum = 0
lines = File.readlines('../i.txt')
arr = lines[0].split(' ').map(&:to_i)

begin_end_index = arr.length - 1

get_next(arr, 0)


puts @@sum



